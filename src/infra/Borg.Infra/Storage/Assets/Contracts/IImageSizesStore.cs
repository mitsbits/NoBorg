using Borg.Infra.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IImageSizesStore<TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<IFileSpec>> PrepareSizes(TKey fileId);

        Task<Uri> PublicUrl(TKey fileId, VisualSize size);
    }
}