using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.ViewComponents
{
    [ViewComponent(Name = "UserSessionSettings")]
    public class UserSessionSettingsViewComponent : ViewComponent
    {
        private readonly IContextAwareUserSession _session;

        public UserSessionSettingsViewComponent(IContextAwareUserSession session)
        {
            _session = session;
        }

        public IViewComponentResult Invoke()
        {
            var model = new UserSessionViewModel()
            {
                UserName = _session.UserName,
                SessionStart = _session.SessionStart,
                UserIdentifier = _session.UserIdentifier,
                MenuIsCollapsed = _session.MenuIsCollapsed(),
                RowsPerPage = _session.RowsPerPage()
            };
            return View(model);
        }
    }

    public class UserSessionViewModel
    {
        public string UserIdentifier { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset SessionStart { get; set; }

        [Display(Name = "Menu collapsed")]
        public bool MenuIsCollapsed { get; set; }

        [Display(Name = "Rows Per Page")]
        public int RowsPerPage { get; set; }
    }
}