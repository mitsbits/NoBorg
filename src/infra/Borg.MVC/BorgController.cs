using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.MVC
{
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
}