using System;
using Borg.Infra.DDD;

namespace Borg.Infra.Storage.Assets.Contracts
{


    public interface IAssetInfo<out TKey> : IAssetInfo, IDocumentBehaviour, IEntity<TKey> where TKey : IEquatable<TKey>
    {

    }

    public interface IAssetInfo
    {

        IVersionInfo CurrentFile { get; }
        string Name { get; }
    }


}