using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.System
{
    public class Sentinel : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMediator _internallDispatcher;
        private readonly IHostingEnvironment _environment;
        private readonly IServiceProvider _locator;

        public Sentinel(ILoggerFactory logerFactory, IMediator internallDispatcher, IHostingEnvironment environment, IServiceProvider locator)
        {
            _internallDispatcher = internallDispatcher;
            _environment = environment;
            _locator = locator;
            _logger = logerFactory.CreateLogger(GetType());

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Sentiner started & executing.");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Sentiner starting.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Sentiner toping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
