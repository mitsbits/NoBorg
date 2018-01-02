using Borg.Infra.DDD;

namespace Borg.Cms.Basic.Lib.Features.Device
{
    public class SlotRecord : IEntity<int>
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int Ordinal { get; set; }
        public int SectionId { get; set; }
        public string ModuleDecriptorJson { get; set; }
        public string ModuleGender { get; set; }
        public string ModuleTypeName { get; set; }
        public virtual SectionRecord Section { get; set; }
    }
}