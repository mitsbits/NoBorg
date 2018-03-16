using Borg.Infra.Services.BackgroundServices;
using Hangfire.Storage;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.System
{
    public interface ISentinel
    {
        Task<string> FireAndForget<TJob>(params string[] args) where TJob : IEnqueueJob;

        Task<string> Schedule<TJob>(DateTimeOffset executeAt, params string[] args) where TJob : IEnqueueJob;

        Task Recur<TJob>(string jobHandle, string cronExpression, TimeZoneInfo timeZoneInfo, params string[] args) where TJob : IEnqueueJob;

        Task<(JobData job, StateData state)> JobData(string jobHandle);
    }
}