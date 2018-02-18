using Borg.Infra.Services.BackgroundServices;
using Borg.Infra.Storage.Assets.Contracts;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.BackgroundJobs
{
    public class CacheStaticImagesForWeb : IEnqueueJob
    {
        private readonly IStaticImageCacheStore<int> _staticImageCache;

        public CacheStaticImagesForWeb(IStaticImageCacheStore<int> staticImageCache)
        {
            _staticImageCache = staticImageCache;
        }

        public async Task Execute(string[] args)
        {
            var id = int.Parse(args[0]);
            await _staticImageCache.PrepareSizes(id);
        }
    }
}