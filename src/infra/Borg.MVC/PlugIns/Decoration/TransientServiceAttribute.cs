using Microsoft.Extensions.DependencyInjection;
using System;

namespace Borg.MVC.PlugIns.Decoration
{
    public sealed class TransientServiceAttribute : ServiceAttribute
    {
        public TransientServiceAttribute(Type serviceType) : base(serviceType)
        {
        }

        public override ServiceLifetime ServiceLifetime => ServiceLifetime.Transient;
    }
}