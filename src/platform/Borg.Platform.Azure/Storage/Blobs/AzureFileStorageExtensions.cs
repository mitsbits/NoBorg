using Borg.Infra.Storage;
using Borg.Infra.Storage.Contracts;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            return new FileSpecDefinition(blob.Uri.AbsoluteUri, blob.Name, created, created, default(DateTime?), blob.Properties.Length);
        }
    }
}