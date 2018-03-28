using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    public class ConfigurationBlocksController : BackofficeController
    {
        public ConfigurationBlocksController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }
    }
}
