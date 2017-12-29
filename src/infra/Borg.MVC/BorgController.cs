using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.MVC
{
    public abstract class BorgBaseController : Controller
    {
        [NonAction]
        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(Url.Content("~/"));
        }
    }

    public abstract class BorgController : BorgBaseController
    {
        protected readonly ILogger Logger;

        protected BorgController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            ViewBag.DeviceInfo = null;
            ViewBag.ContentInfo = null;
        }

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

        [NonAction]
        protected virtual void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [NonAction]
        protected virtual void AddErrors(CommandResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }

    internal static class FrameworkControllerExtensions
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