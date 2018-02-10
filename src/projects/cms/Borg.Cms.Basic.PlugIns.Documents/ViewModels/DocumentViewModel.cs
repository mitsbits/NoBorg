using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Infra.Storage.Assets;
using System.Linq;
using DocumentState = Borg.Cms.Basic.PlugIns.Documents.Data.DocumentState;

namespace Borg.Cms.Basic.PlugIns.Documents.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentState Document { get; set; }
        public AssetInfoDefinition<int> Asset { get; set; }

        public virtual bool InProgress => Asset.InProgress();

        public virtual DocumentCheckOutState CurrentCheckOut()
        {
            if (!InProgress) return null;
            return Document.CheckOuts.Single(x => x.CheckedIn == false);
        }
    }
}