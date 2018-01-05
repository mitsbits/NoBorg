using System;
using System.Text;
using System.Threading.Tasks;
using Borg.Infra;

namespace Borg
{
    public static class SerializerExtensions
    {
        public static async Task<string> SerializeToStringAsync(this ISerializer serializer, object value)
        {
            if (value == null)
                return null;

            return Encoding.UTF8.GetString(await serializer.SerializeAsync(value).AnyContext());
        }

        public static string SerializeToString(this ISerializer serializer, object value)
        {
            return value == null
                ? null
                : Encoding.UTF8.GetString(AsyncHelpers.RunSync(() => serializer.SerializeAsync(value)));
        }

        public static byte[] Serialize(this ISerializer serializer, object value)
        {
            return value == null ? null : AsyncHelpers.RunSync(() => serializer.SerializeAsync(value));
        }

        public static Task<object> DeserializeAsync(this ISerializer serializer, string data, Type objectType)
        {
            return serializer.DeserializeAsync(Encoding.UTF8.GetBytes(data ?? string.Empty), objectType);
        }

        public static async Task<T> DeserializeAsync<T>(this ISerializer serializer, byte[] data)
        {
            return (T)await serializer.DeserializeAsync(data, typeof(T)).AnyContext();
        }

        public static Task<T> DeserializeAsync<T>(this ISerializer serializer, string data)
        {
            return DeserializeAsync<T>(serializer, Encoding.UTF8.GetBytes(data ?? string.Empty));
        }

        public static object Deserialize(this ISerializer serializer, string data, Type objectType)
        {
            return AsyncHelpers.RunSync(() =>
                serializer.DeserializeAsync(Encoding.UTF8.GetBytes(data ?? string.Empty), objectType));
        }

        public static T Deserialize<T>(this ISerializer serializer, byte[] data)
        {
            var output = default(T);

            var task = Task.Run(async () => { output = (T)await serializer.DeserializeAsync(data, typeof(T)); });

            Task.WaitAll(task);
            return output;
        }

        public static T Deserialize<T>(this ISerializer serializer, string data)
        {
            return AsyncHelpers.RunSync(() =>
            {
                var t = DeserializeAsync<T>(serializer, Encoding.UTF8.GetBytes(data ?? string.Empty));
                return t;
            });
        }
    }
}