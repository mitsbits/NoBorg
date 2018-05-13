using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;

using Borg.MVC.Services.Breadcrumbs;
using Borg.MVC.Services.UserSession;
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
            content.Title = title;
            content.Subtitle = subtitle;
            this.SetContent(content);
        }

        #endregion Page Content

        #region Pager

        private const string pageNumerVariableName = "p";
        private const string rowCountVariableName = "r";

        private RequestPager _pager;

        protected RequestPager Pager(IUserSession session)
        {
            if (_pager != null) return _pager;
            var p = 1;

            if (!string.IsNullOrWhiteSpace(Request.Query[pageNumerVariableName]))
                if (!int.TryParse(Request.Query[pageNumerVariableName], out p)) p = 1;
            session.TryContextualize(this);

            var r = session.RowsPerPage();

            if (!string.IsNullOrWhiteSpace(Request.Query[rowCountVariableName]))
                int.TryParse(Request.Query[rowCountVariableName], out r);

            session.RowsPerPage(r);

            _pager = new RequestPager() { Current = p, RowCount = r };
            return _pager;
        }

        protected class RequestPager
        {
            public int Current { get; internal set; }
            public int RowCount { get; internal set; }
        }

        #endregion Pager

        #region Nreadcrumbs

        protected virtual void Breadcrumbs(params BreadcrumbItem[] Breadcrumb)
        {
            var device = PageDevice<Device>();
            device.Breadcrumbs.AddRange(Breadcrumb);
            PageDevice(device);
        }

        #endregion Nreadcrumbs

        #region AddErrors

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

        #endregion AddErrors
    }
}