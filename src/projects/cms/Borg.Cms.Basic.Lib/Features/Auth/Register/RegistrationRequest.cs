using Borg.Infra.DDD;
using Borg.Infra.DDD.Contracts;
using Borg.Infra.Services;
using Newtonsoft.Json;
using System;

namespace Borg.Cms.Basic.Lib.Features.Auth.Register
{
    public class RegistrationRequest : IHasCompositeKey<string>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => CompositeKey.Row;
            set => CompositeKey = CompositeKey<string>.Create(CompositeKey.Partition, value);
        }

        public CompositeKey<string> CompositeKey { get; set; } = CompositeKey<string>.Create(string.Empty, Randomize.String(7));

        public string Email
        {
            get => CompositeKey.Partition;
            set => CompositeKey = CompositeKey<string>.Create(value, CompositeKey.Row);
        }

        public long SK { get; set; }

        public DateTimeOffset SubmitedOn { get; set; }
    }
}