namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IPageContentAccessor<out TPage> where TPage : IPageContent
    {
        TPage Page { get; }
    }
}