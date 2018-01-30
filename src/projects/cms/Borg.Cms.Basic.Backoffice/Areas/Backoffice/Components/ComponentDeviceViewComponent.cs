using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.Editors;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components
{
    [ViewComponent(Name = "ComponentDevice")]
    public class ComponentDeviceViewComponent : ViewComponent
    {
        private readonly IDeviceStructureProvider _deviceStructureProvider;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public ComponentDeviceViewComponent(IDeviceStructureProvider deviceStructureProvider, IUnitOfWork<CmsDbContext> uow)
        {
            _deviceStructureProvider = deviceStructureProvider;
            _uow = uow;
        }

        public async Task<IViewComponentResult> InvokeAsync(int componentId)
        {
            var link = await _uow.Context.ComponentDeviceStates.AsNoTracking().FirstOrDefaultAsync(x => x.ComponentId == componentId);
            var devId = link?.DeviceId ?? default(int);
            var options = await _uow.QueryRepo<DeviceState>().Find(x => true,
                SortBuilder.Get<DeviceState>().Add(x => x.Theme).Add(x => x.Layout).Build());
            var dict = options.GroupBy(x => x.Theme).ToDictionary(g => string.IsNullOrWhiteSpace(g.Key) ? "[NO THEME]" : g.Key, g => g.ToDictionary(r => r.Id.ToString(), r => r.FriendlyName) as IDictionary<string, string>);
            var dd = new DropDown(devId.ToString(), dict);
            var model = new ComponentDeviceViewModel(componentId, dd);
            return View(model);
        }
    }

    public class ComponentDeviceViewModel
    {
        public ComponentDeviceViewModel()
        {
            
        }
        public ComponentDeviceViewModel(int componentId, DropDown deviceId)
        {
            ComponentId = componentId;
            DeviceId = deviceId;
        }

        public int ComponentId { get; set; }
        [DisplayName("Device")]
        public DropDown DeviceId { get; set; }


    }
}
