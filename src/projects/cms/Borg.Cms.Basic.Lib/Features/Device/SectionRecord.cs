using System.Collections.Generic;
using Borg.Infra.DDD;
using Borg.MVC.BuildingBlocks;

namespace Borg.Cms.Basic.Lib.Features.Device
{
    public class SectionRecord : IEntity<int>
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public int DeviceId { get; set; }
        public string FriendlyName { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<SlotRecord> Slots { get; set; } = new HashSet<SlotRecord>();
        public virtual DeviceRecord Device { get; set; }
    }
}