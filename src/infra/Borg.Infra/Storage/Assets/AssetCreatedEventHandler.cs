using System;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets
{
    public delegate Task AssetCreatedEventHandler<TKey>(AssetCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;
}