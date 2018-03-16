using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Borg.Infra.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Borg.MVC.BuildingBlocks
{
    public abstract class ViewComponentModule<TData> : ViewComponent, IModule<TData> where TData : IDictionary<string, string>
    {
        public ModuleGender ModuleGender => ModuleGender.ViewComponent;
        

   
    }

    public abstract class TidingsViewComponentModule<TTidings> : ViewComponentModule<TTidings> where TTidings : Tidings
    {
        protected readonly ILogger _logger;

        protected TidingsViewComponentModule(ILoggerFactory loggerFactory)
        {
            _logger = (loggerFactory == null) ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }
    }
}