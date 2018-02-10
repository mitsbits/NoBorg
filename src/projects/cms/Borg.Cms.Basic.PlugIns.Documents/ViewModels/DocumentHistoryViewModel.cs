using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Infra.Storage.Assets.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Cms.Basic.PlugIns.Documents.ViewModels
{
    public class DocumentHistoryViewModel
    {
        public DocumentHistoryViewModel(List<IVersionInfo> versions, List<DocumentCheckOutState> checkOuts)
        {
            Versions = versions;
            CheckOuts = checkOuts;
        }

        public List<IVersionInfo> Versions { get; }
        public List<DocumentCheckOutState> CheckOuts { get; }

        public IEnumerable<(IVersionInfo version, DocumentCheckOutState checkOut)> Rows()
        {
            var q = from v in Versions
                    join c in CheckOuts on v.Version equals c.CheckOutVersion
                    into cs
                    from c in cs.DefaultIfEmpty()
                    orderby v.Version descending,
                    c?.CheckedOutOn ?? DateTimeOffset.UtcNow descending,
                    (c != null) ? c.CheckedinOn ?? c.CheckedOutOn : DateTimeOffset.UtcNow descending
                    select (version: v, checkOut: c);

            return q.ToArray();
        }

        public int CurrentVersion()
        {
            return CheckOuts.Where(x => x.CheckedIn).Max(x => x.CheckOutVersion);
        }
    }
}