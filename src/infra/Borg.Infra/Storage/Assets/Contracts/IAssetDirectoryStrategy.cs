using System;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IAssetDirectoryStrategy<in TKey> where TKey : IEquatable<TKey>
    {
        Task<string> ParentFolder(IAssetInfo<TKey> asset);
    }
}