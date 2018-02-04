using Borg.Infra.Storage.Assets;
using DocumentState = Borg.Cms.Basic.PlugIns.Documents.Data.DocumentState;

namespace Borg.Cms.Basic.PlugIns.Documents.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentState Document { get; set; }
        public AssetInfoDefinition<int> Asset { get; set; }
    }
}