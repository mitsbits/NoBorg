using Borg.Infra.Services.BackgroundServices;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.System
{
    public interface ISentinel
    {
        Task FireAndForget<TJob>(params string[] args) where TJob : IEnqueueJob;

        Task Schedule<TJob>(DateTimeOffset executeAt, params string[] args) where TJob : IEnqueueJob;

        Task Recur<TJob>(string jobHandle, string cronExpression, TimeZoneInfo timeZoneInfo, params string[] args) where TJob : IEnqueueJob;
    }
}