using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Infra
{
    public class JsonNetSerializer : ISerializer
    {
        protected readonly JsonSerializerSettings _settings;

        public JsonNetSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings(){ TypeNameHandling = TypeNameHandling.Auto };
        }

        public Task<object> DeserializeAsync(byte[] value, Type objectType)
        {
            return Task.FromResult(JsonConvert.DeserializeObject(Encoding.UTF8.GetString(value), objectType, _settings));
        }

        public Task<byte[]> SerializeAsync(object value)
        {
            return value == null
                ? Task.FromResult<byte[]>(null)
                : Task.FromResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, _settings)));
        }
    }
}