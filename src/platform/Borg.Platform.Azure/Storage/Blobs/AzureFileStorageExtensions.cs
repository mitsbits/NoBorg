using Borg.Infra.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Borg.Infra.Storage.Contracts;

namespace Borg.Platform.Azure.Storage.Blobs
{
    internal static partial class AzureFileStorageExtensions
    {
        internal static IEnumerable<CloudBlockBlob> MatchesPattern(this IEnumerable<CloudBlockBlob> blobs, Regex patternRegex)
        {
            return blobs.Where(blob => patternRegex == null || patternRegex.IsMatch(ToFileInfo(blob).Name));
        }

        internal static IFileSpec ToFileInfo(this CloudBlockBlob blob)
        {
            if (blob.Properties.Length == -1)
                return null;
            var created = blob.Properties.LastModified?.UtcDateTime ?? default(DateTime); //TODO: this sucks big time
            return new FileSpec(blob.SnapshotQualifiedUri.ToString(), blob.Name, created, created, default(DateTime?), blob.Properties.Length);
        }
    }
}