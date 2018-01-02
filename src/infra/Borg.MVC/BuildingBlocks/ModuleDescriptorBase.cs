using Borg.Infra.DTO;

namespace Borg.MVC.BuildingBlocks
{
    public abstract class ModuleDescriptorBase
    {
        public abstract string FriendlyName { get; }
        public abstract string Summary { get; }
        public abstract string ModuleGroup { get; }
        public abstract ModuleGender ModuleGender { get; }
        public Tidings Parameters => GetDefaults().Clone();

        protected abstract Tidings GetDefaults();
    }
}