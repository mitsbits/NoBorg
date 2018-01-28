using System.Collections.Generic;
using Borg.CMS;
using Borg.CMS.BuildingBlocks;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{
    public class DeviceState : IEntity<int>
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string Layout { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<SectionState> Sections { get; set; } = new HashSet<SectionState>();
    }
}