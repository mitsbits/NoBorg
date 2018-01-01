using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.Extensions
{
    public static class DeviceExtensions
    {
        public static string RelativePath(this IDevice device)
        {
            return $"{device.Path}{device.QueryString}";
        }
    }
}