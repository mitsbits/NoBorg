using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Cms.Basic.Lib.Features
{
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }

    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
