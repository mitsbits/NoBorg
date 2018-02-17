using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.ViewModels
{
    public class MimeTypeGroupimgRowViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecordId { get; set; }
        public int MimeTypesCount { get; set; }
    }
}
