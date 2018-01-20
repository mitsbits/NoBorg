using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice
{
    class BackofficeModuleDescriptor : IIModuleDescriptor
    {
        public string Area => "Backoffice";
        public string[] ModuleThemes => new[] { "Backoffice" };
    }
}
