using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Domain
{
    public class CommonModule : Module
    {
        private readonly AppSettings _settings;
        public CommonModule(AppSettings settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
