using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using Borg.Infra.Services.BackgroundServices;
using Borg.Infra.Storage.Assets.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
            if (message.MimeType == "image/jpeg")
            {
              await  _sentinel.FireAndForget<CacheJpgJob>(message.FileId.ToString());
            }
        }
    }

    public class CacheJpgJob : IEnqueueJob
    {
        private readonly IImageSizesStore<int> _imageSizes;

        public CacheJpgJob(IImageSizesStore<int> imageSizes)
        {
            _imageSizes = imageSizes;
        }

        public async Task Execute(string[] args)
        {
            var id = int.Parse(args[0]);
            throw new NotImplementedException();
        }
    }
}
