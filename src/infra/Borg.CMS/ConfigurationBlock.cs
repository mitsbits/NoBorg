using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.CMS
{
    public class ConfigurationBlock<TSetting> : IConfigurationBlock where TSetting : ISetting
    {
        public string Display { get; }

        public string BlockType => typeof(ISetting).FullName;
        public ISetting Setting { get; }
    }
}