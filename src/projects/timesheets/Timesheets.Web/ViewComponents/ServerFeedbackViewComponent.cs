using System.Collections.Generic;
using System.Linq;
using Borg.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.ViewComponents
{
    public class ServerFeedbackViewComponent : ViewComponent
    {
        private readonly IApplicationUserSession _session;
        public ServerFeedbackViewComponent(IApplicationUserSession session)
        {
            _session = session;
        }


        public  IViewComponentResult Invoke()
        {
            if(!_session.Messages.Any()) return View( new IServerResponse[0]);
            var list = new List<IServerResponse>();
            var imported = false;
            while (!imported)
            {
                var sr = _session.Pop();
                if (sr == null)
                {
                    imported = true;
                }
                else
                {
                    list.Add(sr);
                }
            }
            return View( list );
        }
    }
}