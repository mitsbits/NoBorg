using Borg.MVC.PlugIns.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.Discovery.Contracts
{
    public interface IPlugInEfEntityRegistration : IPluginDescriptor
    {
        IDictionary<Type, Func<ModelBuilder, bool>> Entities { get; }
    }
}