namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDevice : IHaveAUrl, IHaveAController, ICanRenderAParentView, ICanContextualize, IDeviceStrctureInfo
    {
        string FriendlyName { get; }
  
    }
}