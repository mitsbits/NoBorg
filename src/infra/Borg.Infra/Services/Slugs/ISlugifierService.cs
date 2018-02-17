using System;

namespace Borg.Infra.Services.Slugs
{
    public interface ISlugifierService : IStringSlugifierService, IDateSlugifierService
    {
        
    }
    public interface IStringSlugifierService
    {
        string Slugify(string source, int maxlength = 42);
    }
    public interface IDateSlugifierService
    {
        string Slugify(DateTime source);
        bool TryDeSlugify(string slug, out DateTime date);
    }
}