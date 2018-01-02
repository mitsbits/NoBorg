using Borg.Infra.DDD;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.Features.Device
{
    public class DeviceRecord : IEntity<int>, IHaveALayout
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string Layout { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<SectionRecord> Sections { get; set; } = new HashSet<SectionRecord>();
    }
}