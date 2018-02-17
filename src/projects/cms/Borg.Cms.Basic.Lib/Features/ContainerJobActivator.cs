using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Borg.Cms.Basic.Lib.Features
{
    public class ContainerJobActivator : JobActivator
    {
        private readonly IServiceProvider _locator;

        public ContainerJobActivator(IServiceProvider locator)
        {
            _locator = locator;
        }

        public override object ActivateJob(Type jobType)
        {
            using (var scope = _locator.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService(jobType);
            }
                  
        }
    }
}