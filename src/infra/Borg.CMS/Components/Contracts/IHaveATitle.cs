using System.Collections.Generic;

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

    public interface IHavePrimaryImage
    {
        IDictionary<string, string> PrimaryImages { get; }
    }

    public interface IHaveMainContent
    {
        string MainContent { get; }
    }

    public interface IHavePagedContent : IHaveMainContent
    {
        string[] MainContentPages { get; }
    }
}