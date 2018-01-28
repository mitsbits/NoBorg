using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Conventions;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Cms.Basic.Presentation.Controllers
{
    [PlugInEntryPointController("Presentation")]
    [ControllerTheme("Bootstrap3")]
    public class PresentationController : Controller
    {
        #region Page Content

        [NonAction]
        protected virtual TContent PageContent<TContent>(TContent content = default(TContent)) where TContent : IPageContent
        {
            if (content == null || content.Equals(default(TContent))) return this.GetContent<TContent>();
            this.SetContent(content);
            return content;
        }

        [NonAction]
        protected virtual TDevice PageDevice<TDevice>(TDevice device = default(TDevice)) where TDevice : IDevice
        {
            if (device == null || device.Equals(default(TDevice))) return this.GetDevice<TDevice>();
            this.SetDevice(device);
            return device;
        }

        [NonAction]
        protected virtual void SetPageTitle(string title, string subtitle = "")
        {
            var content = this.GetContent<PageContent>();
            content.SetTitle(title);
            content.Subtitle = subtitle;
            this.SetContent(content);
        }

        #endregion Page Content
    }

    internal static class PresentationControllerExtensions
    {
        public static TContent GetContent<TContent>(this PresentationController controller) where TContent : IPageContent
        {
            var page = controller.ViewBag.ContentInfo as IPageContent ?? new PageContent { Title = "default" };
            return (TContent)page;
        }

        public static void SetContent<TContent>(this PresentationController controller, TContent content) where TContent : IPageContent
        {
            controller.ViewBag.ContentInfo = content;
        }

        public static TDevice GetDevice<TDevice>(this PresentationController controller) where TDevice : IDevice
        {
            var device = controller.ViewBag.DeviceInfo as IDevice ?? new Device { Path = string.Empty, Layout = string.Empty };
            return (TDevice)device;
        }

        public static void SetDevice<TDevice>(this PresentationController controller, TDevice device) where TDevice : IDevice
        {
            controller.ViewBag.DeviceInfo = device;
        }
    }
}