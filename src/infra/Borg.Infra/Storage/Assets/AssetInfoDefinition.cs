using System;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public class AssetInfoDefinition<TKey> : IAssetInfo<TKey>, IAssetInfo where TKey : IEquatable<TKey>
    {
        public AssetInfoDefinition(TKey id, string name)
        {
            Id = id;
            Name = name;
        }

        public IVersionInfo CurrentFile { get; set; }
        public string Name { get; }

        public IVersionInfo CheckOut()
        {
            throw new NotImplementedException();
        }

        public IVersionInfo Checkin(IVersionInfo edit)
        {
            throw new NotImplementedException();
        }

        public DocumentState DocumentState { get; protected set; }
        public TKey Id { get; }
    }
}