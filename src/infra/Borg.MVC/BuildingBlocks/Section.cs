using Borg.MVC.BuildingBlocks.Contracts;
using System.Collections.Generic;
using System.Linq;
using Borg.Infra;

namespace Borg.MVC.BuildingBlocks
{
    public class Section : ISection
    {
        public Section()
        {
            Slots = new HashSet<Slot>();
        }

        public string Identifier { get;  set; }

        public string FriendlyName { get; set; }

        ICollection<ISlot> ISection.Slots => Slots.Cast<ISlot>().ToList();
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<Slot> Slots { get; protected set; }

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