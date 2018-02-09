using System;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Infra.Storage.Assets.Contracts;
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
            var q = from c in CheckOuts
                join v in Versions on c.CheckOutVersion equals v.Version into vs
                from v in vs.DefaultIfEmpty()
                select (version: v, checkOut: c);
            return q.ToArray();

        }

    }
}