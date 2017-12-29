using Borg.Infra.DDD;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Azure.Storage.Tables
{
    public class AzureTableRepository<T> : IAzureTableRepository<T> where T : TableEntity, IHasCompositeKey<string>, new()
    {
        private readonly string _tableName = typeof(T).Name;
        private readonly CloudTable _table;

        public AzureTableRepository(string connectionString, string tableName = "")
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            if (!string.IsNullOrEmpty(tableName)) _tableName = tableName;
            _table = tableClient.GetTableReference(_tableName);
            _table.CreateIfNotExistsAsync().Wait();
        }

        public virtual async Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var t = typeof(T);
            TableOperation insertOperation = TableOperation.Insert(entity);
            bool expanded = false;
            if (entity is IExpandPropertiesToColumns expnd)
            {
                expanded = true;
                insertOperation = TableOperation.Insert(expnd.Expanded());
            }
            var result = await _table.ExecuteAsync(insertOperation);
            if (result.HttpStatusCode == (int)HttpStatusCode.NoContent)
            {
                if (expanded)
                {
                    var d = (DynamicTableEntity)result.Result;
                    var w = GetWarpper(d);
                    return w;
                }
                return (T)result.Result;
            }

            return null;
        }

        private static T GetWarpper(DynamicTableEntity entity)
        {
            var innerType = typeof(T).GenericTypeArguments[0];
            var outerType = typeof(TableJsonEntity<>).MakeGenericType(innerType);
            ConstructorInfo ci = outerType.GetConstructor(new[] { innerType });

            var innerObject = JsonConvert.DeserializeObject(entity.Properties["Data"].StringValue, innerType);

            var returned = ci.Invoke(new[] { innerObject });

            return (T)returned;
        }

        public async Task<IEnumerable<T>> Find(string predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            TableQuery<T> query = new TableQuery<T>().Where(predicate);
            TableContinuationToken continuationToken = null;
            var results = new List<T>();
            do
            {
                var hits = await _table.ExecuteQuerySegmentedAsync(query, continuationToken);
                foreach (var hit in hits)
                {
                    results.Add(hit);
                }

                continuationToken = hits.ContinuationToken;
            } while (continuationToken != null);
            return results;
        }

        public async Task Delete(CompositeKey<string> key, CancellationToken cancellationToken = default(CancellationToken))
        {
            TableEntity entity = default(TableEntity);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(key.Partition, key.Row);
            TableResult retrievedResult = await _table.ExecuteAsync(retrieveOperation);
            if (retrievedResult.HttpStatusCode == (int)HttpStatusCode.OK) entity = (TableEntity)retrievedResult.Result;

            if (entity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(entity);
                TableResult deletedResult = await _table.ExecuteAsync(deleteOperation);
            }
        }

        public async Task<T> Get(CompositeKey<string> key, CancellationToken cancellationToken = default(CancellationToken))
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(key.Partition, key.Row);
            TableResult retrievedResult = await _table.ExecuteAsync(retrieveOperation);
            if (retrievedResult.HttpStatusCode == (int)HttpStatusCode.OK) return (T)retrievedResult.Result;
            return null;
        }
    }
}