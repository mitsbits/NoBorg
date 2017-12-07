using Borg.Infra.Serializer;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Borg.Infra.Tests.Serializer
{
    public class JsonNetSerializerTests
    {
        private const string _objGuid = "89B302BC-25EE-4333-AF6A-75FBB75340EC";
        private const string _objText = "Hi there!";
        private const double _objNumeric = 42.5;
        private readonly ISerializer _serializer;
        private readonly ObjectToSerialize _obj;
        private readonly byte[] _objData;
        private readonly string _objDataString;

        public JsonNetSerializerTests()
        {
            var settings = new JsonSerializerSettings();
            _serializer = new JsonNetSerializer(settings);
            _obj = new ObjectToSerialize() { Id = Guid.Parse(_objGuid), Numeric = _objNumeric, Textual = _objText };
            _objData = new byte[]
            {
                123,
                34,
                78,
                117,
                109,
                101,
                114,
                105,
                99,
                34,
                58,
                52,
                50,
                46,
                53,
                44,
                34,
                73,
                100,
                34,
                58,
                34,
                56,
                57,
                98,
                51,
                48,
                50,
                98,
                99,
                45,
                50,
                53,
                101,
                101,
                45,
                52,
                51,
                51,
                51,
                45,
                97,
                102,
                54,
                97,
                45,
                55,
                53,
                102,
                98,
                98,
                55,
                53,
                51,
                52,
                48,
                101,
                99,
                34,
                44,
                34,
                84,
                101,
                120,
                116,
                117,
                97,
                108,
                34,
                58,
                34,
                72,
                105,
                32,
                116,
                104,
                101,
                114,
                101,
                33,
                34,
                125
            };
            _objDataString = "{\"Numeric\":42.5,\"Id\":\"89b302bc-25ee-4333-af6a-75fbb75340ec\",\"Textual\":\"Hi there!\"}";
        }

        [Fact]
        public async Task SerializeAsyncObjectToByteArray()
        {
            var data = await _serializer.SerializeAsync(_obj);
            _objData.Length.ShouldBe(data.Length);

            for (var i = 0; i < data.Length; i++)
            {
                _objData[i].ShouldBe(data[i]);
            }
        }

        [Fact]
        public async Task DeserializeAsyncByteArrayToObject()
        {
            var obj = await _serializer.DeserializeAsync(_objData, typeof(ObjectToSerialize)) as ObjectToSerialize;

            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }

        [Fact]
        public async Task SerializeToStringAsyncFromObject()
        {
            var data = await _serializer.SerializeToStringAsync(_obj);
            data.ShouldBe(_objDataString);
        }

        [Fact]
        public void SerializeToStringFromObject()
        {
            var data = _serializer.SerializeToString(_obj);
            data.ShouldBe(_objDataString);
        }

        [Fact]
        public void SerializeFromObject()
        {
            var data = _serializer.Serialize(_obj);
            _objData.Length.ShouldBe(data.Length);

            for (var i = 0; i < data.Length; i++)
            {
                _objData[i].ShouldBe(data[i]);
            }
        }

        [Fact]
        public async Task DeserializeAsyncFromString()
        {
            var obj = await _serializer.DeserializeAsync(_objDataString, typeof(ObjectToSerialize)) as ObjectToSerialize;
            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }

        [Fact]
        public async Task DeserializeAsyncTyped()
        {
            var obj = await _serializer.DeserializeAsync<ObjectToSerialize>(_objData);
            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }

        [Fact]
        public async Task DeserializeAsyncFromStringTyped()
        {
            var obj = await _serializer.DeserializeAsync<ObjectToSerialize>(_objDataString);
            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }

        [Fact]
        public void DeserializeFromString()
        {
            var obj = _serializer.Deserialize(_objDataString, typeof(ObjectToSerialize)) as ObjectToSerialize;
            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }

        [Fact]
        public void DeserializeTyped()
        {
            var obj = _serializer.Deserialize<ObjectToSerialize>(_objData);
            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }

        [Fact]
        public void DeserializeFromStringTyped()
        {
            var obj = _serializer.Deserialize<ObjectToSerialize>(_objDataString);
            obj.ShouldNotBeNull();
            obj.Numeric.ShouldBe(_objNumeric);
            obj.Id.ShouldBe(Guid.Parse(_objGuid));
            obj.Textual.ShouldBe(_objText);
        }
    }
}