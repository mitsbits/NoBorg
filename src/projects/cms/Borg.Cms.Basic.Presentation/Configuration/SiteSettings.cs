using Borg.CMS;
using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.Cms.Basic.Presentation.Configuration
{
    public class SiteConfigurationBlock : ConfigurationBlock<SiteSettings>
    {
        public override string Display => "Site Settings";
        public override string SettingType => typeof(SiteSettings).FullName;
    }

    public class SiteSettings : ISetting
    {


    }
}