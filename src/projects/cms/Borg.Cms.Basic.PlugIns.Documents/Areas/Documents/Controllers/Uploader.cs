using Borg.Cms.Basic.PlugIns.Documents.Commands;
using Borg.Infra.Caching.Contracts;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    public class UploaderController : DocumentsController
    {
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly ICacheStore _cache;
        private readonly IMediator _dispatcher;

        public UploaderController(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore, ICacheStore cache, IMediator dispatcher) : base(loggerFactory)
        {
            _assetStore = assetStore;
            _cache = cache;
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Asset(int id)
        {
            var asset = (await _assetStore.Projections(new[] { id })).First();

            var stream = await _assetStore.CurrentFile(id);

            stream.Seek(0, 0);
            return File(stream, asset.CurrentFile.FileSpec.MimeType,
                asset.CurrentFile.FileSpec.Name);
        }

        [HttpGet]
        public async Task<IActionResult> Version(int id, int version)
        {
            var asset = (await _assetStore.Projections(new[] { id })).First();

            using (var stream = await _assetStore.VersionFile(id, version))
            {
                stream.Seek(0, 0);
                return File(stream, asset.CurrentFile.FileSpec.MimeType, asset.CurrentFile.FileSpec.Name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Home(IList<IFormFile> files, string uploadid)
        {
            var pr = new UploadProgress() { UploadId = uploadid, Current = 0, Total = files.Sum(f => f.Length) };

            var cacheKey = $"UPLOAD[{uploadid}]";
            var cacheResultKey = $"UPLOAD[{uploadid}]RESULT";

            await _cache.SetSliding(cacheKey, pr, TimeSpan.FromSeconds(60));

            var bucket = new List<AssetInfoDefinition<int>>();

            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString().Trim('"');

                filename = this.EnsureCorrectFilename(filename);

                byte[] buffer = new byte[16 * 1024];

                using (var output = new MemoryStream())
                {
                    using (Stream input = source.OpenReadStream())
                    {
                        long totalReadBytes = 0;
                        int readBytes;

                        while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            await output.WriteAsync(buffer, 0, readBytes);
                            totalReadBytes += readBytes;
                            pr.Reads(totalReadBytes);
                            await _cache.SetSliding(cacheKey, pr, TimeSpan.FromSeconds(5));
                            await Task.Delay(100); // It is only to make the process slower
                        }
                        var definition = await _assetStore.Create(Path.GetFileNameWithoutExtension(filename), output.ToArray(), filename);
                        bucket.Add(definition);
                    }
                }
            }

            CommandResult commResult = CommandResult.Success();
            foreach (var assetInfoDefinition in bucket)
            {
                commResult = await _dispatcher.Send(new DocumentInitialCommitCommand(assetInfoDefinition.Id, User.Identity.Name));
            }
            if (commResult.Succeded)
            {
                await _cache.Remove(cacheKey);
                await _cache.SetSliding(cacheResultKey, bucket, TimeSpan.FromMinutes(2));
                return Ok(new { name = bucket[0].Name, url = Url.Action("Item", "Home", new { area = "documents", id = bucket[0].Id }) });
            }
            return BadRequest(commResult.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Progress(string uploadid)
        {
            try
            {
                var cacheKey = $"UPLOAD[{uploadid}]";
                var cacheResultKey = $"UPLOAD[{uploadid}]RESULT";
                var pr = await _cache.Get<UploadProgress>(cacheKey);
                if (pr == null)
                {
                    var uploadresult = await _cache.Get<List<AssetInfoDefinition<int>>>(cacheResultKey);
                    if (uploadresult != null)
                    {
                        var output = new { result = true, payload = uploadresult };
                        return Json(output);
                    }
                    return Json(new { val = "0%" });
                }
                return Json(new { val = pr.Current.ToString() });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = true, val = "-1" });
            }
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private class UploadProgress
        {
            public string UploadId { get; set; }
            public long Total { get; set; }
            public long Current { get; set; }

            public void Reads(long totalReadBytes)
            {
                Current = (long)((float)totalReadBytes / (float)Total * 100.0);
            }
        }
    }
}