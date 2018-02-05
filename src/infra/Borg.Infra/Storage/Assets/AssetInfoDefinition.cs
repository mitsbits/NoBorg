using Borg.Infra.Serializer;
using Borg.Infra.Storage.Assets.Contracts;
using Newtonsoft.Json;
using System;

namespace Borg.Infra.Storage.Assets
{
    public class AssetInfoDefinition<TKey> : IAssetInfo<TKey>, IAssetInfo where TKey : IEquatable<TKey>
    {
        public AssetInfoDefinition(TKey id, string name, DocumentBehaviourState state = DocumentBehaviourState.Commited)
        {
            Id = id;
            Name = name;
            DocumentBehaviourState = state;
        }

        [JsonConverter(typeof(ConcreteConverter<VersionInfoDefinition>))]
        public IVersionInfo CurrentFile { get; set; }

        public string Name { get; }

        public DocumentBehaviourState DocumentBehaviourState { get; protected set; } = DocumentBehaviourState.Commited;
        public TKey Id { get; }

        public virtual bool InProgress() => DocumentBehaviourState == DocumentBehaviourState.InProgress;
    }
}