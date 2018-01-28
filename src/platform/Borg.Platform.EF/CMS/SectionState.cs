using System.Collections.Generic;
using Borg.CMS;
using Borg.CMS.BuildingBlocks;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{
    public class SectionState : IEntity<int>
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public int DeviceId { get; set; }
        public string FriendlyName { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<SlotState> Slots { get; set; } = new HashSet<SlotState>();
        public virtual DeviceState Device { get; set; }
    }
}