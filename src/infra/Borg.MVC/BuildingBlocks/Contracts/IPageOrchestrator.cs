namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IPageOrchestrator<out TPage, out TDevice> :
        IPageOrchestrator,
        IPageContentAccessor<IPageContent>,
        IDeviceAccessor<IDevice>
        where TPage : IPageContent where TDevice : IDevice
    {
    }

    public interface IPageOrchestrator : ICanContextualize
    {
    }
}