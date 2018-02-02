using Borg.Infra.DTO;
using Borg.MVC.PlugIns.Contracts;

namespace Borg.Cms.Basic.Presentation
{
    public sealed class PresentationPlugInDescriptor : IPluginDescriptor, IPlugInTheme, IPlugInArea
    {
        public string Title => "Presentation";
        public string[] Themes => new[] { "Bootstrap3" };
        public string Area => Title;
        public Tidings BackofficeEntryPointAction => new Tidings();
    }
}