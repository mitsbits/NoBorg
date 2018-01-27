using Borg.Cms.Basic.Lib.Features.Content.Events;
using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Content.Commands
{
    public class ContentItemCreateOrUpdateCommand : CommandBase<CommandResult<ContentItemRecord>>
    {
        public ContentItemCreateOrUpdateCommand()
        {
        }

        public ContentItemCreateOrUpdateCommand(string title, string slug, string subtitle, string body, DateTimeOffset publishDate, string author, DateTimeOffset? lastRevisionDate, int recordId = 0)
        {
            Title = title;
            Slug = slug;
            Subtitle = subtitle;
            Body = body;
            PublishDate = publishDate;
            Author = author;
            LastRevisionDate = lastRevisionDate;
            RecordId = recordId;
        }

        [Required]
        public int RecordId { get; set; }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Subtitle")]
        public string Subtitle { get; set; }

        [Required]
        [DisplayName("Slug")]
        public string Slug { get; set; }

        [DisplayName("Body")]
        [UIHint("CKEDITOR")]
        public string Body { get; set; }

        [DisplayName("Author")]
        public string Author { get; set; }

        [Required]
        [DisplayName("Publish Date")]
        public DateTimeOffset PublishDate { get; set; }

        [DisplayName("Last Revision")]
        public DateTimeOffset? LastRevisionDate { get; set; }
    }

    public class ContentItemCreateOrUpdateCommandHandler : AsyncRequestHandler<ContentItemCreateOrUpdateCommand, CommandResult<ContentItemRecord>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<BorgDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly IDeviceLayoutFileProvider _deviceLayoutFiles;

        public ContentItemCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher, IDeviceLayoutFileProvider deviceLayoutFiles)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _deviceLayoutFiles = deviceLayoutFiles;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<ContentItemRecord>> HandleCore(ContentItemCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<ContentItemRecord>();
                ContentItemRecord record;
                if (isTransient)
                {
                    record = new ContentItemRecord() { Author = message.Author, Body = message.Body, LastRevisionDate = message.LastRevisionDate, PublisheDate = message.PublishDate, Slug = message.Slug, Subtitle = message.Subtitle, Title = message.Title };

                    await repo.Create(record);
                    await _uow.Save();
                    _logger.Info("Created content item {@record}", record);
                    await _dispatcher.Publish(new ContentItemRecordStateChangedEvent(record.Id, DmlOperation.Create));
                    return CommandResult<ContentItemRecord>.Success(record);
                }

                record = await repo.Get(x => x.Id == message.RecordId);
                if (record == null)
                    return CommandResult<ContentItemRecord>.FailureWithEmptyPayload(
                        $"No content item found for id {message.RecordId}");
                _uow.Context.Entry(record).State = EntityState.Detached;
                var update = new ContentItemRecord()
                {
                    Id = message.RecordId,
                    Subtitle = message.Subtitle,
                    Author = message.Author,
                    Title = message.Title,
                    Slug = message.Slug,
                    Body = message.Body,
                    LastRevisionDate = message.LastRevisionDate,
                    PublisheDate = message.PublishDate
                };
                _uow.Context.Attach(update);
                _uow.Context.Entry(update).State = EntityState.Modified;
                await _uow.Save();
                await _dispatcher.Publish(new DeviceRecordStateChanged(record.Id, DmlOperation.Update));
                return CommandResult<ContentItemRecord>.Success(update);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating/updating content item from {@message} - {exception}", message, ex.ToString());
                return CommandResult<ContentItemRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}