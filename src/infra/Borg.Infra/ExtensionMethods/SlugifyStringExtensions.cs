using Borg.Infra.Services.Slugs;

namespace Borg
{
    public static class SlugifyStringExtensions
    {
        private static readonly ISlugifierService _service;

        static SlugifyStringExtensions()
        {
            _service =  Slugifier.CreateDefault();
        }


        public static string Slugify(this string source)
        {
            return _service.Slugify(source, -1);
        }
    }
}