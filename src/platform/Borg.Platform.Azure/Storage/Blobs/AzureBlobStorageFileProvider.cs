using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Borg.Infra;
using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace Borg.Platform.Azure.Storage.Blobs
{
    public class AzureBlobStorageFileProvider : IFileProvider
    {
        private readonly IFileStorage _storage;
        private readonly string _scope;

        public AzureBlobStorageFileProvider(IFileStorage storage, string scope)
        {
            Preconditions.NotNull(storage, nameof(storage));
            Preconditions.NotEmpty(scope, nameof(scope));
            _storage = storage;
            _scope = scope;

        }

        private IFileStorage Scope()
        {
            return _storage.Scope(_scope);
        }
        public IFileInfo GetFileInfo(string subpath)
        {
            var spec = AsyncHelpers.RunSync(async () => await _storage.GetFileInfo(subpath));

            return new AzureStorageBlobFileInfo(spec, Scope());
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return null;
        }

        public IChangeToken Watch(string filter)
        {
            return null;
        }
    }


    public class AzureStorageBlobFileInfo : IFileInfo
    {
        private readonly IFileSpec _spec;
        private readonly IFileStorage _storage; //TODO: this is not cool, must move from costructor to a service some how

        public AzureStorageBlobFileInfo(IFileSpec spec, IFileStorage storage)
        {
            _spec = spec;
            _storage = storage;
        }

        public Stream CreateReadStream()
        {
            var stream = new MemoryStream();
            using (var source = AsyncHelpers.RunSync(async () => await _storage.GetFileStream(_spec.Name)))
            {
                source.Seek(0, 0);
                source.CopyTo(stream);
            }
            stream.Seek(0, 0);
            return stream;
        }

        public bool Exists => AsyncHelpers.RunSync(() => _storage.Exists(_spec.Name));
        public long Length => _spec.SizeInBytes;
        public string PhysicalPath => _spec.FullPath;
        public string Name => _spec.Name;
        public DateTimeOffset LastModified => _spec.LastWrite;
        public bool IsDirectory => false;
    }
}
