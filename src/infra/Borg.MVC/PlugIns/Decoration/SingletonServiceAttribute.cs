using Microsoft.Extensions.DependencyInjection;
using System;

namespace Borg.MVC.PlugIns.Decoration
{
    public sealed class SingletonServiceAttribute : ServiceAttribute
    {
        public SingletonServiceAttribute(Type serviceType) : base(serviceType)
        {
        }

        public override ServiceLifetime ServiceLifetime => ServiceLifetime.Singleton;
    }
}