using Borg.CMS.BuildingBlocks.Contracts;
using Borg.Infra;
using System.Collections.Generic;
using System.Linq;

namespace Borg.CMS.BuildingBlocks
{
    public class Section : ISection
    {
        public Section()
        {
            Slots = new HashSet<Slot>();
        }

        public string Identifier { get; set; }

        public string FriendlyName { get; set; }

        ICollection<ISlot> ISection.Slots => Enumerable.Cast<ISlot>(Slots).ToList();
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<Slot> Slots { get; set; }

        public void DefineSlot(SectionSlotInfo info, ModuleRenderer module)
        {
            Preconditions.NotNull(info, nameof(info));
            Preconditions.NotNull(module, nameof(module));
            var slot = Slots.FirstOrDefault(x => x.SectionSlotInfo.Ordinal == info.Ordinal
                                     && x.SectionSlotInfo.SectionIdentifier == info.SectionIdentifier);

            if (slot != null) Slots.Remove(slot);

            Slots.Add(new Slot(info, module));
        }
    }
}