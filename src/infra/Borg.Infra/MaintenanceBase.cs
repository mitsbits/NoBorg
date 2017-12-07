using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Infra
{
    public class MaintenanceBase : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory;
        private ScheduledTimer _maintenanceTimer;

        public MaintenanceBase(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            Logger = _loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void InitializeMaintenance(TimeSpan? dueTime = null, TimeSpan? intervalTime = null)
        {
            _maintenanceTimer = new ScheduledTimer(DoMaintenanceAsync, _loggerFactory, dueTime, intervalTime);
        }

        protected void ScheduleNextMaintenance(DateTime utcDate)
        {
            Logger.LogDebug("Scheduling {maintenance} next for {utcDate}", GetType(), utcDate);
            _maintenanceTimer.ScheduleNext(utcDate);
        }

        protected virtual Task<DateTime?> DoMaintenanceAsync()
        {
            return Task.FromResult<DateTime?>(DateTime.MaxValue);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _maintenanceTimer?.Dispose();
        }
    }
}