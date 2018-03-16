using Borg.Infra.Services;
using Borg.Infra.Services.BackgroundServices;
using Borg.MVC.PlugIns.Contracts;
using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.System
{
    public class Sentinel : BackgroundService, ISentinel
    {
        private readonly ILogger _logger;
        private readonly IMediator _internallDispatcher;
        private readonly IHostingEnvironment _environment;
        private readonly IServiceProvider _locator;
        private readonly IPlugInHost _plugInHost;

        public Sentinel(ILoggerFactory logerFactory, IMediator internallDispatcher, IHostingEnvironment environment, IServiceProvider locator, IPlugInHost plugInHost)
        {
            _logger = logerFactory.CreateLogger(GetType());
            _internallDispatcher = internallDispatcher;
            _environment = environment;
            _locator = locator;
            _plugInHost = plugInHost;
        }

        #region IHostedService

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
            _logger.Info("Sentiner stoping.");
            return base.StopAsync(cancellationToken);
        }

        #endregion IHostedService

        public Task<string> FireAndForget<TJob>(params string[] args) where TJob : IEnqueueJob
        {
            try
            {
                var jobHandle = BackgroundJob.Enqueue<TJob>(j => j.Execute(args));
                return Task.FromResult(jobHandle);
            }
            catch (Exception e)
            {
                _logger.Warn("Failed to run {job} with {@args} - reason: {exception}", typeof(TJob).Name, args, e.Message);
                return Task.FromResult(string.Empty);
            }
        }

        public Task<string> Schedule<TJob>(DateTimeOffset executeAt, params string[] args) where TJob : IEnqueueJob
        {
            var jobHandle = BackgroundJob.Schedule<TJob>(j => j.Execute(args).AnyContext(), executeAt);
            return Task.FromResult(jobHandle);
        }

        public Task Recur<TJob>(string jobHandle, string cronExpression, TimeZoneInfo timeZoneInfo, params string[] args) where TJob : IEnqueueJob
        {
            RecurringJob.AddOrUpdate<TJob>(jobHandle, j => j.Execute(args).AnyContext(), cronExpression, timeZoneInfo);
            return Task.CompletedTask;
        }



        public Task<(JobData job, StateData state)> JobData(string jobHandle)
        {
            IStorageConnection connection = JobStorage.Current.GetConnection();
            var job = connection.GetJobData(jobHandle);
            var state = connection.GetStateData(jobHandle);
            return Task.FromResult((job: job, state: state));
        }
    }
}