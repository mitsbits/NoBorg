using Borg.CMS;
using Borg.CMS.BackOfficeInstructions;
using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.Cms.Basic.Presentation.Configuration
{
    public class SiteConfigurationBlock : ConfigurationBlock<SiteSettings>
    {
        public override string Display => "Site Settings";
        public override string SettingType => typeof(SiteSettings).AssemblyQualifiedName;
    }

    public class SiteSettings : ISetting
    {
        public string SampleProperty1 { get; set; }


        [EditorTab("foo")]
        public string SampleProperty2 { get; set; }

    }



}