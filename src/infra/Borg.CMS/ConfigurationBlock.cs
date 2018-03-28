using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.CMS
{
    public abstract class ConfigurationBlock<TSetting> : IConfigurationBlock where TSetting : ISetting
    {
        public virtual string Display { get; }

        public abstract string SettingType { get; }
        public virtual ISetting Setting { get; }

    }
}