using Borg.CMS.BuildingBlocks;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.CMS;

namespace Borg
{
    public static class DeviceStateExtensions
    {
        public static DeviceStructureInfo DeviceStructureInfo(this DeviceState hit)
        {
            var result = new DeviceStructureInfo()
            {
                RenderScheme = hit.RenderScheme,
                Layout = hit.Layout
            };
            foreach (var sectionRecord in hit.Sections)
            {
                var section = new Section()
                {
                    FriendlyName = sectionRecord.FriendlyName,
                    Identifier = sectionRecord.Identifier,
                    RenderScheme = sectionRecord.RenderScheme
                };
                foreach (var slotRecord in sectionRecord.Slots)
                {
                    var values = slotRecord.Module(sectionRecord.Identifier);
                    section.DefineSlot(values.slotInfo, values.renderer);
                }
                result.Sections.Add(section);
            }
            return result;
        }
    }
}