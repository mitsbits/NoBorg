using Microsoft.Extensions.DependencyInjection;
using System;

namespace Borg.MVC.PlugIns.Decoration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class ServiceAttribute : Attribute
    {
        protected ServiceAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }
        public abstract ServiceLifetime ServiceLifetime { get; }
    }
}