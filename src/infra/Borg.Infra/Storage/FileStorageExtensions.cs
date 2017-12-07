using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Borg.Infra.Storage
{
    public static class FileStorageExtensions
    {
        public static Stream GetFileStream(this IFileStorage storage, string path)
        {
            return AsyncHelpers.RunSync(() => storage.GetFileStream(path));
        }

        public static IFileSpec GetFileInfo(this IFileStorage storage, string path)
        {
            return AsyncHelpers.RunSync(() => storage.GetFileInfo(path));
        }

        public static bool Exists(this IFileStorage storage, string path)
        {
            return AsyncHelpers.RunSync(() => storage.Exists(path));
        }

        public static bool SaveFile(this IFileStorage storage, string path, Stream stream)
        {
            return AsyncHelpers.RunSync(() => storage.SaveFile(path, stream));
        }

        public static bool RenameFile(this IFileStorage storage, string path, string newpath)
        {
            return AsyncHelpers.RunSync(() => storage.RenameFile(path, newpath, default(CancellationToken)));
        }

        public static bool CopyFile(this IFileStorage storage, string path, string targetpath)
        {
            return AsyncHelpers.RunSync(() => storage.CopyFile(path, targetpath));
        }

        public static bool DeleteFile(this IFileStorage storage, string path)
        {
            return AsyncHelpers.RunSync(() => storage.DeleteFile(path));
        }

        public static IEnumerable<IFileSpec> GetFileList(this IFileStorage storage, string searchPattern = null,
            int? limit = null, int? skip = null)
        {
            return AsyncHelpers.RunSync(() => storage.GetFileList(searchPattern, limit, skip));
        }
    }
}