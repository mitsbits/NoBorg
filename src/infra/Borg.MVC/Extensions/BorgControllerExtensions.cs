using Borg.MVC;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg
{
    internal static class BorgControllerExtensions
    {
        public static TContent GetContent<TContent>(this BorgController controller) where TContent : IPageContent
        {
            var page = controller.ViewBag.ContentInfo as IPageContent ?? new PageContent { Title = "default" };
            return (TContent)page;
        }

        public static void SetContent<TContent>(this BorgController controller, TContent content) where TContent : IPageContent
        {
            controller.ViewBag.ContentInfo = content;
        }

        public static TDevice GetDevice<TDevice>(this BorgController controller) where TDevice : IDevice
        {
            var device = controller.ViewBag.DeviceInfo as IDevice ?? new Device { Path = string.Empty, Layout = string.Empty };
            return (TDevice)device;
        }

        public static void SetDevice<TDevice>(this BorgController controller, TDevice device) where TDevice : IDevice
        {
            controller.ViewBag.DeviceInfo = device;
        }
    }
}