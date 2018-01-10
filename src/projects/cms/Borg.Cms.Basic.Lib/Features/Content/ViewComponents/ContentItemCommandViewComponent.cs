using Borg.Cms.Basic.Lib.Features.Content.Commands;
using Borg.Cms.Basic.Lib.Features.Content.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;

namespace Borg.Cms.Basic.Lib.Features.Content.ViewComponents
{
    public class ContentItemCommandViewComponent : ViewComponent
    {
        private readonly IMediator _dispatcher;
        private readonly ILogger _logger;
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;

        public ContentItemCommandViewComponent(ILoggerFactory loggerFactory, IMediator dispatcher, IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            _dispatcher = dispatcher;
            _orchestrator = orchestrator;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<IViewComponentResult> InvokeAsync(int recordId = 0)
        {
            ContentItemCreateOrUpdateCommand command;
            if (recordId <= 0)
            {
                command = new ContentItemCreateOrUpdateCommand("", "", "", "", DateTimeOffset.UtcNow, "", default(DateTimeOffset?));
            }
            else
            {
                var result = await _dispatcher.Send(new ContentItemRequest(recordId));
                if (result.Succeded && result.Payload != null)
                {
                    var record = result.Payload;
                    command = new ContentItemCreateOrUpdateCommand(record.Title, record.Slug, record.Subtitle,
                        record.Body, record.PublisheDate, record.Author, record.LastRevisionDate, recordId);
                    _orchestrator.TryContextualize(ViewContext);
                    _orchestrator.Page.SetTitle(record.Title);
                }
                else
                {
                    command = new ContentItemCreateOrUpdateCommand("", "", "", "", DateTimeOffset.UtcNow, "", default(DateTimeOffset?));
                }
            }
            return View(command);
        }
    }
}