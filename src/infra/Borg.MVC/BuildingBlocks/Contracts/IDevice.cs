namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDevice : IHaveAUrl, IHaveAController, ICanRenderToAView, IHaveALayout, ICanContextualize, IHaveSections
    {
        string FriendlyName { get; }
    }
}