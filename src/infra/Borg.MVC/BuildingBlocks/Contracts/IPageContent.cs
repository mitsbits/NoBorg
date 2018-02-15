namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IPageContent
    {
        HtmlMetaSet Metas { get; }
        string Title { get; }
        string Subtitle { get; }
        string[] Body { get; }
        void SetTitle(string title);
    }
}