using System;
using System.Threading.Tasks;

namespace Borg.CMS.Documents.Contracts
{
    public interface IDocumentsService<TKey> where TKey : IEquatable<TKey>
    {
        Task<TKey> StoreUserDocument(byte[] data, string filename, string userHandle);
    }
}