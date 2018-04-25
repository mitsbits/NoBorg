using Borg.Infra.Storage.Contracts;
using System;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IVersionInfo
    {
        int Version { get; }

        IFileSpec FileSpec { get; }
    }

    public interface IVersionInfo<out TKey> : IVersionInfo where TKey : IEquatable<TKey>
    {
        new IFileSpec<TKey> FileSpec { get; }
    }
}