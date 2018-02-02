using Borg.CMS.BuildingBlocks;
using Borg.CMS.BuildingBlocks.Contracts;
using Borg.MVC.BuildingBlocks.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.BuildingBlocks
{
    public class DeviceStructureInfo : IDeviceStructureInfo
    {
        public ICollection<Section> Sections { get; set; } = new HashSet<Section>();
        ICollection<ISection> IHaveSections.Sections => Sections.Cast<ISection>().ToList();
        public string RenderScheme { get; set; }

        public void SectionsClear()
        {
            Sections.Clear();
        }

        public void SectionAdd(ISection section)
        {
            Sections.Add(section as Section);
        }

        public string Layout { get; set; }
    }
}