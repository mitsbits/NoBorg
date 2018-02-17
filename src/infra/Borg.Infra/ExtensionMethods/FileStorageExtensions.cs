using Borg.Infra;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Borg
{
    public static class FileStorageExtensions
    {
        internal static readonly string mimesPath = "Borg.Infra.Storage.mimes.json";
        internal static readonly IDictionary<string, string> _mappings;

        static FileStorageExtensions()
        {
            var assembly = typeof(FileStorageExtensions).Assembly;
            using (var resource = assembly.GetManifestResourceStream(mimesPath))
            {
                using (var reader = new StreamReader(resource))
                {
                    var context = reader.ReadToEnd();
                    _mappings = JsonConvert.DeserializeObject<Dictionary<string, string>>(context);
                }
            }
        }

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

        public static string GetMimeType(this string extensionOrPath)
        {
            if (string.IsNullOrWhiteSpace(extensionOrPath))
                throw new ArgumentNullException(nameof(extensionOrPath));

            var seperator = '.';
            var seperatorHit = extensionOrPath.IndexOf(seperator);
            var hasDoubleSeperator = seperatorHit > -1 && seperatorHit != extensionOrPath.LastIndexOf(seperator);
            var doubleDotted = _mappings.Keys.Where(x => x.Count(s => s.Equals(seperator)) > 1);
            var doubleHit = doubleDotted.FirstOrDefault(extensionOrPath.EndsWith);
            if (hasDoubleSeperator)
            {
                if (doubleHit != null) return _mappings[doubleHit];
            }
            else
            {
                doubleHit = doubleDotted.FirstOrDefault(x => (seperator + extensionOrPath).EndsWith(x));
                if (doubleHit != null) return _mappings[doubleHit];
            }
            var extensionFromPath = Path.GetExtension(extensionOrPath);
            var extension = string.IsNullOrWhiteSpace(extensionFromPath) ? extensionOrPath : extensionFromPath;
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            if (!extension.StartsWith(seperator.ToString()))
                extension = seperator + extension;

            return _mappings.TryGetValue(extension, out var mime) ? mime : "application/octet-stream";
        }

        public static IFileSpec ToSpec(this FileInfo info, string textToRemoveFomFullPath = "")
        {
            var fullPath = string.IsNullOrWhiteSpace(textToRemoveFomFullPath)
                ? info.FullName
                : info.FullName.Replace(textToRemoveFomFullPath, string.Empty);
            return new FileSpecDefinition(fullPath, info.Name, info.CreationTimeUtc, info.LastWriteTimeUtc,
                info.LastAccessTimeUtc, info.Length);
        }

        public static IFileStorage Scope(this IFileStorage fileStorage, string scope)
        {
            if (fileStorage == null) throw new ArgumentNullException(nameof(fileStorage));
            if (string.IsNullOrWhiteSpace(scope)) throw new ArgumentNullException(nameof(scope));
            return new ScopedFileStorage(fileStorage, scope);
        }

        public static async Task<bool> RenameFile(this IFileStorage fileStorage, string path, string newpath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await fileStorage.CopyFile(path, newpath, cancellationToken) && await fileStorage.DeleteFile(path, cancellationToken);
        }

        public static async Task<bool> SaveFile(this IFileStorage fileStorage, string path, string content, CancellationToken cancellationToken = default(CancellationToken))
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content ?? ""));
            return await fileStorage.SaveFile(path, stream, cancellationToken);
        }
    }

    internal class SystemMimesProvider
    {
        private static readonly IDictionary<string, string> _mappings;

        static SystemMimesProvider()
        {
            var assembly = typeof(SystemMimesProvider).Assembly;
            using (var resource = assembly.GetManifestResourceStream(FileStorageExtensions.mimesPath))
            {
                using (var reader = new StreamReader(resource))
                {
                    var context = reader.ReadToEnd();
                    _mappings = JsonConvert.DeserializeObject<Dictionary<string, string>>(context);
                }
            }
        }

        public IDictionary<string, string> Mappings => _mappings;
    }
}