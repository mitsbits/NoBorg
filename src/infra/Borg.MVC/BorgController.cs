using System;
using System.Collections.Generic;
using System.Text;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Borg.Infra.DAL;
using Microsoft.AspNetCore.Identity;

namespace Borg.MVC
{
    public abstract class BorgController : Controller
    {
        protected readonly ILogger Logger;

        protected BorgController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
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

        [NonAction]
        protected virtual IActionResult RedirectToLocal(string returnUrl, string homeController = "Home", string homeAction = "Home")
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(homeController, "Home");
            }
        }
    }

    internal static class FrameworkControllerExtensions
    {
        private static readonly IPageContent _defaultContent = new PageContent { Title = "default" };
        private static readonly IDevice _defaultDevice = new Device { Path = string.Empty, Layout = string.Empty };

        public static TContent GetContent<TContent>(this BorgController controller) where TContent : IPageContent
        {
            var page = controller.ViewBag.ContentInfo as IPageContent ?? _defaultContent;
            return (TContent)page;
        }

        public static void SetContent<TContent>(this BorgController controller, TContent content) where TContent : IPageContent
        {
            controller.ViewBag.ContentInfo = content;
        }

        public static TDevice GetDevice<TDevice>(this BorgController controller) where TDevice : IDevice
        {
            var device = controller.ViewBag.DeviceInfo as IDevice ?? _defaultDevice;
            return (TDevice)device;
        }

        public static void SetDevice<TDevice>(this BorgController controller, TDevice device) where TDevice : IDevice
        {
            controller.ViewBag.DeviceInfo = device;
        }
    }
}
