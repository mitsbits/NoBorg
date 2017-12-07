namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDeviceAccessor<out TDevice> : ICanContextualize where TDevice : IDevice
    {
        TDevice Device { get; }
    }
}