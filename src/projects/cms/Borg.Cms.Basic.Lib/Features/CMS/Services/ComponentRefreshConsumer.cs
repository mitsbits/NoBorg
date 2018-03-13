using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Borg.Cms.Basic.Lib.Features.CMS.Services
{
    public class ComponentRefreshConsumer : INotificationHandler<ArticleBodyChangedEvent>, INotificationHandler<ArticleHtmlMetasChangedEvent>, INotificationHandler<ArticlePrimaryImageChangedEvent>
        , INotificationHandler<ArticleRenamedEvent>
    {
        private readonly IComponentPageDescriptorService<int> _descriptors;
        private readonly ILogger _logger;

        public ComponentRefreshConsumer(IComponentPageDescriptorService<int> descriptors, ILoggerFactory loggerFactory)
        {
            _descriptors = descriptors;
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }
        public async Task Handle(ArticleBodyChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Info("Invalidating Component {id} - {event}", notification.ArticleId, notification.GetType().Name);
            await Invalidate(notification.ArticleId);
        }
        public async Task Handle(ArticleHtmlMetasChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Info("Invalidating Component {id} - {event}", notification.ArticleId, notification.GetType().Name);
            await Invalidate(notification.ArticleId);
        }


        public async Task Handle(ArticleRenamedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Info("Invalidating Component {id} - {event}", notification.Id, notification.GetType().Name);
            await Invalidate(notification.Id);
        }
        public async Task Handle(ArticlePrimaryImageChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Info("Invalidating Component {id} - {event}", notification.RecordId, notification.GetType().Name);
            await Invalidate(notification.RecordId);
        }
        private async Task Invalidate(int id)
        {
            await _descriptors.Invalidate(id);
        }



    }
}
