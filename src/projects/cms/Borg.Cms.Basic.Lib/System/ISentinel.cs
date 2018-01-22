using System;
using System.Threading.Tasks;
using Borg.Infra.Services.BackgroundServices;

namespace Borg.Cms.Basic.Lib.System
{
    public interface ISentinel
    {
        Task FireAndForget<TJob>(TJob job, params string[] args) where TJob : IEnqueueJob;
        Task Schedule<TJob>(TJob job, DateTimeOffset executeAt, params string[] args) where TJob : IEnqueueJob;
        Task Recur<TJob>(TJob job, string jobHandle, string cronExpression, TimeZoneInfo timeZoneInfo, params string[] args) where TJob : IEnqueueJob;
    }
}