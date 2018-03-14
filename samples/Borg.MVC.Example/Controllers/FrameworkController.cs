using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Mvc.Example.Controllers
{
    public class FrameworkController : Controller
    {
        protected TContent PageContent<TContent>(TContent content = default(TContent)) where TContent : IPageContent
        {
            if (content == null || content.Equals(default(TContent))) return this.GetContent<TContent>();
            this.SetContent(content);
            return content;
        }

        protected TDevice PageDevice<TDevice>(TDevice device = default(TDevice)) where TDevice : IDevice
        {
            if (device == null || device.Equals(default(TDevice))) return this.GetDevice<TDevice>();
            this.SetDevice(device);
            return device;
        }

        protected void SetPageTitle(string title, string subtitle = "")
        {
            var content = this.GetContent<PageContent>();
            content.Title = title;
            content.Subtitle = subtitle;
            this.SetContent(content);
        }
    }

    internal static class FrameworkControllerExtensions
    {
        private static readonly IPageContent _defaultContent = new PageContent { Title = "default" };
        private static readonly IDevice _defaultDevice = new Device { FriendlyName = "Default", Path = string.Empty };

        public static TContent GetContent<TContent>(this FrameworkController controller) where TContent : IPageContent
        {
            var page = controller.ViewBag.ContentInfo as IPageContent ?? _defaultContent;
            return (TContent)page;
        }

        public static void SetContent<TContent>(this FrameworkController controller, TContent content) where TContent : IPageContent
        {
            controller.ViewBag.ContentInfo = content;
        }

        public static TDevice GetDevice<TDevice>(this FrameworkController controller) where TDevice : IDevice
        {
            var device = controller.ViewBag.DeviceInfo as IDevice ?? _defaultDevice;
            return (TDevice)device;
        }

        public static void SetDevice<TDevice>(this FrameworkController controller, TDevice device) where TDevice : IDevice
        {
            controller.ViewBag.DeviceInfo = device;
        }
    }
}