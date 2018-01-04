using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Borg.Cms.Basic.Lib.Features.Device.Commands;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Borg.Cms.Basic.Lib.Features.Device.ViewModels
{
    public class SectionViewModel
    {
        public SectionRecord Record { get; set; }
        public IEnumerable<IModuleDescriptor> Descriptors { get; set; }

        public SlotCreateOrUpdateCommand  SlotCommand => new SlotCreateOrUpdateCommand(true, 0, Record.Id, "","","");

        public string[] AvailableSectionIdentifiers { get; set; }

      static  dynamic cont(IModuleDescriptor d)
        {
            dynamic obj = new ExpandoObject();
            obj.FriendlyName = d.FriendlyName;
            obj.Summary = d.Summary;
            obj.ModuleGroup = d.ModuleGroup;
            obj.ModuleGender = d.ModuleGender.Flavor;
            var dd = d as dynamic;
            var p = dd.Parameters as Tidings;
            var tids = new List<Tiding>();
            foreach (var tid in p)
            {
                tids.Add(tid);
            }
            obj.Parameters = tids.ToArray();
            return obj;
        }

        public Dictionary<string, Tiding> ObjectJsonDict()
        {
            var result = new Dictionary<string, Tiding>();
      
            foreach (var moduleDescriptor in Descriptors)
            {
                string key = moduleDescriptor.GetType().FullName.Replace(".", "");
                string value = JsonConvert.SerializeObject(cont(moduleDescriptor));
                Tiding tiding = new Tiding(key, value){HumanKey = moduleDescriptor.GetType().FullName , Hint = moduleDescriptor.ModuleGender.Flavor};
                result.Add(key, tiding);
            }
   





            return result;
        }
        public IEnumerable<SelectListItem> ModuleOptions()
        {
            yield return new SelectListItem() { Text ="...", Value = "empty" };
            foreach (var descriptor in Descriptors)
            {
                yield return new SelectListItem(){Text = descriptor.FriendlyName, Value = descriptor.GetType().FullName.Replace(".",""), Group =new SelectListGroup(){ Name = descriptor.ModuleGroup}};
            }
        }
    }
}
