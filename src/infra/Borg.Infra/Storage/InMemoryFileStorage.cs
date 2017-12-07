using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Storage
{
    public class InMemoryFileStorage : IFileStorage
    {
        private readonly AsyncLock _lock = new AsyncLock(); //new object();

        private readonly Dictionary<string, Tuple<IFileSpec, byte[]>> _storage =
            new Dictionary<string, Tuple<IFileSpec, byte[]>>(StringComparer.OrdinalIgnoreCase);

        public InMemoryFileStorage() : this(1024 * 1024 * 256, 100)
        {
        }

        public InMemoryFileStorage(long maxFileSize, int maxFiles)
        {
            MaxFileSize = maxFileSize;
            MaxFiles = maxFiles;
        }

        public long MaxFileSize { get; set; }
        public long MaxFiles { get; set; }

        public async Task<Stream> GetFileStream(string path,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            using (await _lock.LockAsync())
            {
                return !_storage.ContainsKey(path) ? null : new MemoryStream(_storage[path].Item2);
            }
        }

        public async Task<IFileSpec> GetFileInfo(string path,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var exists = await Exists(path, cancellationToken);
            if (!exists) return null;
            using (await _lock.LockAsync())
            {
                return _storage[path].Item1;
            }
        }

        public async Task<bool> Exists(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (await _lock.LockAsync())
            {
                return _storage.ContainsKey(path);
            }
        }

        public async Task<bool> SaveFile(string path, Stream stream,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            var contents = ReadBytes(stream);
            if (contents.Length > MaxFileSize)
                throw new ArgumentException(
                    $"File size {contents.Length.SizeDisplay()} exceeds the maximum size of {MaxFileSize.SizeDisplay()}.");

            using (await _lock.LockAsync())
            {
                _storage[path] =
                    Tuple.Create(
                        new FileSpec(path, Path.GetFileName(path), DateTime.UtcNow, DateTime.UtcNow, default(DateTime?),
                            contents.Length) as IFileSpec, contents);

                if (_storage.Count > MaxFiles)
                    _storage.Remove(_storage.OrderByDescending(kvp => kvp.Value.Item1.CreationDate).First().Key);
            }

            return true;
        }

        public async Task<bool> CopyFile(string path, string targetpath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrWhiteSpace(targetpath))
                throw new ArgumentNullException(nameof(targetpath));

            using (await _lock.LockAsync())
            {
                if (!_storage.ContainsKey(path))
                    return false;

                _storage[targetpath] = _storage[path];
                _storage[targetpath].Item1.ModifyPath(targetpath);
            }

            return true;
        }

        public async Task<bool> DeleteFile(string path,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            using (await _lock.LockAsync())
            {
                if (!_storage.ContainsKey(path))
                    return false;

                _storage.Remove(path);
            }

            return true;
        }

        public async Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null,
            int? skip = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (limit.HasValue && limit.Value <= 0)
                return new List<IFileSpec>();

            if (searchPattern == null)
                searchPattern = "*";

            var regex = new Regex("^" + Regex.Escape(searchPattern).Replace("\\*", ".*?") + "$");
            using (await _lock.LockAsync())
            {
                return _storage.Keys.Where(k => regex.IsMatch(k))
                    .Select(k => _storage[k].Item1).Skip(skip ?? 0).Take(limit ?? int.MaxValue).ToList();
            }
        }

        public void Dispose()
        {
            _storage?.Clear();
        }

        private static byte[] ReadBytes(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}