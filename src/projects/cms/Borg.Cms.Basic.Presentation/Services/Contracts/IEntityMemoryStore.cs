using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using Borg.Infra;
using Borg.Platform.EF.CMS;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Borg.Cms.Basic.Presentation.Services.Contracts
{
    public interface IEntityMemoryStore
    {
        IReadOnlyList<NavigationItemState> NavigationItems { get; }
    }

    public class EntityMemoryStore : IEntityMemoryStore
    {
        private static ReaderWriterLock _rwl = new ReaderWriterLock();
        private const int _timeOut = 100;

        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;

        private readonly List<NavigationItemState> _navigationItems = new List<NavigationItemState>();

        public EntityMemoryStore(ILoggerFactory loggerFactory, IMediator dispatcher)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _dispatcher = dispatcher;
            Init();
        }

        public IReadOnlyList<NavigationItemState> NavigationItems => _navigationItems;

        public void Init()
        {
            AsyncHelpers.RunSync(async () =>
            {
                try
                {
                    var items = await _dispatcher.Send(new AllNavigationItemsRequest());
                    _rwl.AcquireReaderLock(_timeOut);
                    if (items.Succeded)
                    {
                        _navigationItems.AddRange(items.Payload);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
                finally
                {
                    _rwl.ReleaseReaderLock();
                }
            });
        }
    }
}