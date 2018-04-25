using Borg.Infra.DDD.Contracts;
using System;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IAssetInfo<out TKey> : IAssetInfo, IDocumentBehaviour, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        new IVersionInfo<TKey> CurrentFile { get; }
    }

    public interface IAssetInfo
    {
        IVersionInfo CurrentFile { get; }
        string Name { get; }
    }
}