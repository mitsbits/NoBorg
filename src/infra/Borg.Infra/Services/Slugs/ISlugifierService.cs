namespace Borg.Infra.Services.Slugs
{
    public interface ISlugifierService
    {
        string Slugify(string source, int maxlength = 42);
    }
}