namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IPageContent : IHaveHtmlMetas, IHaveTags, IHaveComponentKey, ICanHaveADocumentAsPrimaryImage
    {
        string Title { get; }
        string Subtitle { get; }
        string[] Body { get; }

        void SetTitle(string title);
    }
}