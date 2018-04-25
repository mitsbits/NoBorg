using Borg.Infra.Storage.Contracts;
using System;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Documents
{
    public delegate Task FileCreatedEventHandler<in TKey>(IFileSpec<TKey> file) where TKey : IEquatable<TKey>;

    public interface IDocumentsService<TKey> where TKey : IEquatable<TKey>
    {
        event FileCreatedEventHandler<TKey> FileCreated;

        Task<(TKey docid, IFileSpec<TKey> file)> StoreUserDocument(byte[] data, string filename, string userHandle);
    }
}