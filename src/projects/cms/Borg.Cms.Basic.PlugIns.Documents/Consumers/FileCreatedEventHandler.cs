using Borg.Cms.Basic.Lib.System;
using Borg.Cms.Basic.PlugIns.Documents.BackgroundJobs;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Consumers
{
    public class FileCreatedEventHandler : AsyncNotificationHandler<FileCreatedEvent>
    {
        private readonly ILogger _logger;
        private readonly ISentinel _sentinel;

        public FileCreatedEventHandler(ILoggerFactory loggerFactory, ISentinel sentinel)
        {
            _sentinel = sentinel;

            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        protected override async Task HandleCore(FileCreatedEvent message)
        {
            await _sentinel.FireAndForget<CacheStaticImagesForWeb>(message.FileId.ToString());
        }
    }
}