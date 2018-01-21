using System;
using System.Collections.Generic;
using System.Text;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Cms.Basic.Lib.Discovery.Contracts
{
    public interface IPlugInEfEntityRegistration : IPluginDescriptor
    {
        IDictionary<Type, Func<ModelBuilder, bool>> Entities { get; }
    }


}
