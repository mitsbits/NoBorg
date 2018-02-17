using Borg.Cms.Basic.Lib.Features.Device.Commands;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Borg.Cms.Basic.Lib.Features.Device.ViewModels
{
    public class SectionViewModel
    {
        public SectionState State { get; set; }
        public IEnumerable<IModuleDescriptor> Descriptors { get; set; }

        public SlotCreateOrUpdateCommand SlotCommand => new SlotCreateOrUpdateCommand(true, 0, State.Id, "", "", "");

        public string[] AvailableSectionIdentifiers { get; set; }

        private static dynamic cont(IModuleDescriptor d)
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
                Tiding tiding = new Tiding(key, value) { HumanKey = moduleDescriptor.GetType().FullName, Hint = moduleDescriptor.ModuleGender.Flavor };
                result.Add(key, tiding);
            }

            return result;
        }

        public IEnumerable<SelectListItem> ModuleOptions()
        {
            yield return new SelectListItem() { Text = "...", Value = "empty" };
            var groups = Descriptors.Select(x => x.ModuleGroup).Distinct()
                .ToDictionary(x => x, x => new SelectListGroup() { Name = x });
            foreach (var descriptor in Descriptors)
            {
                yield return new SelectListItem() { Text = descriptor.FriendlyName, Value = descriptor.GetType().FullName.Replace(".", ""), Group = groups[descriptor.ModuleGroup] };
            }
        }
    }
}