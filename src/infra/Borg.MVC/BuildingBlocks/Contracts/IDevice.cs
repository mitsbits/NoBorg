namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDevice : IHaveAFriendlyName, IHaveAUrl, IHaveAController, ICanRenderParentViewElements, ICanContextualize, IDeviceStructureInfo
    {
    }
}