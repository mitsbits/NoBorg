using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Storage
{
    public interface IFileStorage : IDisposable
    {
        Task<Stream> GetFileStream(string path, CancellationToken cancellationToken = default(CancellationToken));

        Task<IFileSpec> GetFileInfo(string path, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Exists(string path, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SaveFile(string path, Stream stream,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> CopyFile(string path, string targetpath,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> DeleteFile(string path, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null, int? skip = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IScopedFileStorage : IFileStorage
    {
        string Scope { get; }
    }
}