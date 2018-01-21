using Microsoft.Extensions.DependencyInjection;
using System;

namespace Borg.MVC.PlugIns.Decoration
{
    public sealed class ScopedServiceAttribute : ServiceAttribute
    {
        public ScopedServiceAttribute(Type serviceType) : base(serviceType)
        {
        }

        public override ServiceLifetime ServiceLifetime => ServiceLifetime.Scoped;
    }
}