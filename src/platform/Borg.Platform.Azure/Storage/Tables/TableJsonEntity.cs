using Borg.Infra.DDD;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Borg.Platform.Azure.Storage.Tables
{
    public class TableJsonEntity<T> : TableEntity, IExpandPropertiesToColumns, IHasPartitionKey<string> where T : IHasPartitionKey<string>
    {
        internal TableJsonEntity(string partitionKey, string rowKey, string jsonData)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Data = jsonData;
        }

        public TableJsonEntity(T inner)
        {
            var key = inner.PartitionedKey;
            PartitionKey = key.Partition;
            RowKey = key.Row;
            Data = JsonConvert.SerializeObject(inner);
        }

        public TableJsonEntity()
        {
            Debug.WriteLine($"{typeof(T).FullName} is de-serialized");
        }

        public string Data { get; set; }
        public PartitionedKey<string> PartitionedKey => PartitionedKey<string>.Create(PartitionKey, RowKey);

        public T Payload()
        {
            try
            {
                var wrapper = JsonConvert.DeserializeObject(Data, typeof(T));
                return (T)wrapper;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public ITableEntity Expanded()
        {
            try
            {
                var wrapper = JsonConvert.DeserializeObject(Data, typeof(T));
                var props = typeof(T).GetProperties().Where(x => x.MemberType == MemberTypes.Property);
                var dict = new Dictionary<string, object>();
                foreach (var propertyInfo in props)
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    {
                        var value = propertyInfo.GetValue(wrapper);
                        dict.Add(propertyInfo.Name, value);
                    }
                }

                dict[nameof(PartitionKey)] = PartitionKey;
                dict[nameof(RowKey)] = RowKey;
                dict[nameof(ETag)] = ETag;
                dict[nameof(Data)] = Data;

                return GetDynamicTableEntity(PartitionKey, RowKey, ETag, dict);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private static DynamicTableEntity GetDynamicTableEntity(string partition, string row, string etag, Dictionary<string, object> properties)
        {
            return new DynamicTableEntity(partition, row, etag, GetProps(properties));
        }

        private static IDictionary<string, EntityProperty> GetProps(IDictionary<string, object> properties)
        {
            if (properties == null || !properties.Any()) return default(IDictionary<string, EntityProperty>);

            string[] stringColumns = { "Id", "PartitionKey", "RowKey" };
            string[] ignoreColumns = { "PartitionKey", "RowKey", "ETag" };

            var result = new Dictionary<string, EntityProperty>();
            foreach (var property in properties)
            {
                if (ignoreColumns.Contains(property.Key)) continue;

                if (property.Value != null)
                {
                    var handled = false;
                    var isNumeric = double.TryParse(property.Value.ToString(), out var n);
                    if (stringColumns.Contains(property.Key))
                    {
                        result.Add(property.Key, EntityProperty.GeneratePropertyForString(property.Value.ToString()));
                        handled = true;
                    }
                    if (!handled && isNumeric)
                    {
                        handled = true;
                        result.Add(property.Key, EntityProperty.GeneratePropertyForDouble(n));
                    }
                    if (!handled && (property.Value is DateTime || property.Value is DateTimeOffset))
                    {
                        var value = (property.Value is DateTime) ? new DateTimeOffset((DateTime)property.Value) : (DateTimeOffset)property.Value;
                        result.Add(property.Key, EntityProperty.GeneratePropertyForDateTimeOffset(value));
                        handled = true;
                    }
                    if (!handled && property.Value is string)
                    {
                        result.Add(property.Key, EntityProperty.GeneratePropertyForString(property.Value.ToString()));
                        handled = true;
                    }

                    if (!handled && property.Value is Enum)
                    {
                        result.Add(property.Key,
                            EntityProperty.GeneratePropertyForString(((Enum)property.Value).ToString()));
                        handled = true;
                    }

                    if (!handled)
                    {
                        var type = property.Value.GetType();
                        if (type.GetMethod("ToString").DeclaringType == type)
                        {
                            result.Add(property.Key,
                                EntityProperty.GeneratePropertyForString(property.Value.ToString()));
                        }
                        //result.Add(property.Key,
                        //    EntityProperty.GeneratePropertyForString(JsonConvert.SerializeObject(property.Value)));
                    }
                }
                else
                {
                    //result.Add(property.Key,
                    //    EntityProperty.GeneratePropertyForString(JsonConvert.SerializeObject(property.Value)));
                }
            }
            return result;
        }
    }
}