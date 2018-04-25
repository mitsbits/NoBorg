using System;
using System.Threading.Tasks;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;

namespace Borg
{
    public static class AssetDirectoryStrategyExtensions
    {

        public static Task<string> ParentFolder<TKey>(this IAssetDirectoryStrategy<TKey> strategy, TKey key) where TKey : IEquatable<TKey>
        {
            return strategy.ParentFolder(new DummyFileSpec<TKey>(key));
        }
        internal  class DummyFileSpec<TKey> : IFileSpec<TKey> where TKey : IEquatable<TKey>
        {
            internal DummyFileSpec(TKey key)
            {
                Id = key;
            }
            public TKey Id { get; set; }

            public string FullPath => throw new NotImplementedException();

            public string Name => throw new NotImplementedException();

            public DateTime CreationDate => throw new NotImplementedException();

            public DateTime LastWrite => throw new NotImplementedException();

            public DateTime? LastRead => throw new NotImplementedException();

            public long SizeInBytes => throw new NotImplementedException();

            public string MimeType => throw new NotImplementedException();

            public string Extension => throw new NotImplementedException();
            public IFileSpec<TKey> Clone()
            {
                throw new NotImplementedException();
            }
        }
    }
}