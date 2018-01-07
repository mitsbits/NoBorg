using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra.Storage.Contracts;

namespace Borg.Infra.Storage
{
    public class FolderFileStorage : IFileStorage
    {
        private readonly AsyncLock _lock = new AsyncLock();

        public FolderFileStorage(string folder, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory != null ? loggerFactory.CreateLogger(GetType()) : NullLogger.Instance;

            folder = PathHelper.ExpandPath(folder);

            if (!Path.IsPathRooted(folder))
                folder = Path.GetFullPath(folder);
            if (!folder.EndsWith("\\"))
                folder += "\\";

            Folder = folder;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        protected ILogger Logger { get; }

        public string Folder { get; }

        public async Task<Stream> GetFileStream(string path,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            try
            {
                if (!await Exists(path, cancellationToken).AnyContext())
                    return null;
                return File.OpenRead(Path.Combine(Folder, path));
            }
            catch (FileNotFoundException ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Task<IFileSpec> GetFileInfo(string path,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!File.Exists(Path.Combine(Folder, path)))
                return Task.FromResult<IFileSpec>(null);

            var info = new FileInfo(Path.Combine(Folder, path));

            return Task.FromResult(info.ToSpec());
        }

        public Task<bool> Exists(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(File.Exists(Path.Combine(Folder, path)));
        }

        public Task<bool> SaveFile(string path, Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            var directory = Path.GetDirectoryName(Path.Combine(Folder, path));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            try
            {
                using (var fileStream = File.Create(Path.Combine(Folder, path)))
                {
                    if (stream.CanSeek)
                        stream.Seek(0, SeekOrigin.Begin);

                    stream.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        //public async Task<bool> RenameFile(string path, string newpath, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    if (string.IsNullOrWhiteSpace(path))
        //        throw new ArgumentNullException(nameof(path));
        //    if (string.IsNullOrWhiteSpace(newpath))
        //        throw new ArgumentNullException(nameof(newpath));

        //    try
        //    {
        //        using (await _lock.LockAsync(cancellationToken))
        //        {
        //            var directory = Path.GetDirectoryName(newpath);
        //            if (directory != null && !Directory.Exists(Path.Combine(Folder, directory)))
        //                Directory.CreateDirectory(Path.Combine(Folder, directory));

        //            File.Move(Path.Combine(Folder, path), Path.Combine(Folder, newpath));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //        return false;
        //    }

        //    return true;
        //}

        public async Task<bool> CopyFile(string path, string targetpath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrWhiteSpace(targetpath))
                throw new ArgumentNullException(nameof(targetpath));

            try
            {
                using (await _lock.LockAsync())
                {
                    var directory = Path.GetDirectoryName(targetpath);
                    if (directory != null && !Directory.Exists(Path.Combine(Folder, directory)))
                        Directory.CreateDirectory(Path.Combine(Folder, directory));

                    File.Copy(Path.Combine(Folder, path), Path.Combine(Folder, targetpath));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }

            return true;
        }

        public Task<bool> DeleteFile(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            try
            {
                File.Delete(Path.Combine(Folder, path));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null,
            int? skip = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (limit.HasValue && limit.Value <= 0)
                return Task.FromResult<IEnumerable<IFileSpec>>(new List<IFileSpec>());

            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*";

            var list = new List<IFileSpec>();
            if (!Directory.Exists(Path.GetDirectoryName(Path.Combine(Folder, searchPattern))))
                return Task.FromResult<IEnumerable<IFileSpec>>(list);

            list.AddRange(
                from path in Directory.GetFiles(Folder, searchPattern, SearchOption.AllDirectories).Skip(skip ?? 0)
                    .Take(limit ?? int.MaxValue)
                select new FileInfo(path)
                into info
                where info.Exists
                select info.ToSpec());

            return Task.FromResult<IEnumerable<IFileSpec>>(list);
        }

        public void Dispose()
        {
        }
    }

    internal static class PathHelper
    {
        private const string DATA_DIRECTORY = "|DataDirectory|";

        public static string ExpandPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            if (!path.StartsWith(DATA_DIRECTORY, StringComparison.OrdinalIgnoreCase))
                return Path.GetFullPath(path);

            var dataDirectory = GetDataDirectory();
            var length = DATA_DIRECTORY.Length;

            if (path.Length <= length)
                return dataDirectory;

            var relativePath = path.Substring(length);
            var c = relativePath[0];

            if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
                relativePath = relativePath.Substring(1);

            var fullPath = Path.Combine(dataDirectory ?? string.Empty, relativePath);
            fullPath = Path.GetFullPath(fullPath);

            return fullPath;
        }

        public static string GetDataDirectory()
        {
            try
            {
                //string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                //if (String.IsNullOrEmpty(dataDirectory))
                //    dataDirectory = AppDomain.CurrentDomain.BaseDirectory;

                //return Path.GetFullPath(dataDirectory);
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}