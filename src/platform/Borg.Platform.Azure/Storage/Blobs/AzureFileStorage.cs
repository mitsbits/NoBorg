using Borg.Infra.Storage.Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Azure.Storage.Blobs
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly CloudBlobContainer _container;

        public AzureFileStorage(string connectionString, string containerName = "storage")
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();
            _container = client.GetContainerReference(containerName);
            Task.WaitAll(_container.CreateIfNotExistsAsync());
        }

        public async Task<Stream> GetFileStream(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blockBlob = _container.GetBlockBlobReference(path);
            try
            {
                return await blockBlob.OpenReadAsync().AnyContext();
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 404)
                    return null;

                throw;
            }
        }

        public async Task<IFileSpec> GetFileInfo(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blob = _container.GetBlockBlobReference(path);
            try
            {
                await blob.FetchAttributesAsync().AnyContext();
                return blob.ToFileInfo();
            }
            catch (Exception) { }

            return null;
        }

        public Task<bool> Exists(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blockBlob = _container.GetBlockBlobReference(path);
            return blockBlob.ExistsAsync();
        }

        public async Task<bool> SaveFile(string path, Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blockBlob = _container.GetBlockBlobReference(path);
            blockBlob.Properties.ContentType = path.GetMimeType();
            await blockBlob.UploadFromStreamAsync(stream).AnyContext();

            return true;
        }

        public async Task<bool> CopyFile(string path, string targetpath, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var oldBlob = _container.GetBlockBlobReference(path);
            var newBlob = _container.GetBlockBlobReference(targetpath);

            await newBlob.StartCopyAsync(oldBlob).AnyContext();
            while (newBlob.CopyState.Status == CopyStatus.Pending)
                await Task.Delay(50, cancellationToken).AnyContext();

            return newBlob.CopyState.Status == CopyStatus.Success;
        }

        public Task<bool> DeleteFile(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            var blockBlob = _container.GetBlockBlobReference(path);
            return blockBlob.DeleteIfExistsAsync();
        }

        public async Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null, int? skip = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (limit.HasValue && limit.Value <= 0)
                return new List<IFileSpec>();

            searchPattern = searchPattern?.Replace('\\', '/');
            string prefix = searchPattern;
            Regex patternRegex = null;
            int wildcardPos = searchPattern?.IndexOf('*') ?? -1;
            if (searchPattern != null && wildcardPos >= 0)
            {
                patternRegex = new Regex("^" + Regex.Escape(searchPattern).Replace("\\*", ".*?") + "$");
                int slashPos = searchPattern.LastIndexOf('/');
                prefix = slashPos >= 0 ? searchPattern.Substring(0, slashPos) : String.Empty;
            }
            prefix = prefix ?? String.Empty;

            BlobContinuationToken continuationToken = null;
            var blobs = new List<CloudBlockBlob>();
            do
            {
                var listingResult = await _container.ListBlobsSegmentedAsync(prefix, true, BlobListingDetails.Metadata, limit, continuationToken, null, null, cancellationToken).AnyContext();
                continuationToken = listingResult.ContinuationToken;
                blobs.AddRange(listingResult.Results.OfType<CloudBlockBlob>().MatchesPattern(patternRegex));
            } while (continuationToken != null && blobs.Count < limit.GetValueOrDefault(int.MaxValue));

            if (limit.HasValue)
                blobs = blobs.Take(limit.Value).ToList();

            return blobs.Select(blob => blob.ToFileInfo());
        }

        public void Dispose()
        {
            // _container?
        }
    }
}