namespace Borg.CMS.Components.Contracts
{
    public interface IHaveTitle
    {
        string Title { get; }
    }
    public interface IHaveSubtitleTitle
    {
        string Subtitle { get; }
    }
    public interface IHaveASlug
    {
        string Slug { get; }
    }

    public interface IHaveARelativePath
    {
        string RelativePath { get; }
    }

    public interface IHaveAPrimaryImage
    {
        string APrimaryImage { get; }
    }

    public interface IHaveAMainContent
    {
        string MainContent { get; }
    }

    public interface IHaveAPAgedContent : IHaveAMainContent
    {
        string[] MainContent { get; }
    }
}