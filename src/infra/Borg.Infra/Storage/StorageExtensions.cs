using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Storage
{
    internal class SystemMimesProvider
    {
        private static readonly IDictionary<string, string> _mappings;

        static SystemMimesProvider()
        {
            var assembly = typeof(SystemMimesProvider).Assembly;
            using (var resource = assembly.GetManifestResourceStream(StorageExtensions.mimesPath))
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

    public static class StorageExtensions
    {
        internal static readonly string mimesPath = "Borg.Infra.Storage.mimes.json";
        private static readonly IDictionary<string, string> _mappings;

        static StorageExtensions()
        {
            var assembly = typeof(StorageExtensions).Assembly;
            using (var resource = assembly.GetManifestResourceStream(mimesPath))
            {
                using (var reader = new StreamReader(resource))
                {
                    var context = reader.ReadToEnd();
                    _mappings = JsonConvert.DeserializeObject<Dictionary<string, string>>(context);
                }
            }
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

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }

        public static IFileSpec ToSpec(this FileInfo info, string textToRemoveFomFullPath = "")
        {
            var fullPath = string.IsNullOrWhiteSpace(textToRemoveFomFullPath)
                ? info.FullName
                : info.FullName.Replace(textToRemoveFomFullPath, string.Empty);
            return new FileSpec(fullPath, info.Name, info.CreationTimeUtc, info.LastWriteTimeUtc,
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
            return await fileStorage.CopyFile(path, newpath, cancellationToken)
                ? await fileStorage.DeleteFile(path, cancellationToken)
                : false;
        }
    }
}