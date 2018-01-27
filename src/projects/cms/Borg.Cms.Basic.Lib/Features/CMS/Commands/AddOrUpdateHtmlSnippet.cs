using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class AddOrUpdateHtmlSnippetCommand : CommandBase<CommandResult<HtmlSnippetState>>
    {
        [Required]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Code")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Snippet")]
        [UIHint("CKEDITOR")]
        public string Snippet { get; set; }
    }

    public class AddOrUpdateHtmlSnippetCommandHandler : AsyncRequestHandler<AddOrUpdateHtmlSnippetCommand, CommandResult<HtmlSnippetState>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        public AddOrUpdateHtmlSnippetCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<HtmlSnippetState>> HandleCore(AddOrUpdateHtmlSnippetCommand message)
        {
            try
            {
                var isTransient = message.Id == 0;
                var repo = _uow.ReadWriteRepo<HtmlSnippetState>();
                HtmlSnippetState record;
                if (isTransient)
                {
                    var component = new ComponentState();
                    record = new HtmlSnippetState(message.Code, message.Snippet) { Component = component };

                    await repo.Create(record);
                    await _uow.Save();
                    _logger.Info("Created html snippet {@record}", record);

                    return CommandResult<HtmlSnippetState>.Success(record);
                }

                record = await repo.Get(x => x.Id == message.Id);
                if (record == null)
                    return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(
                        $"No html snippet found for id {message.Id}");
                record.Code = message.Code;
                record.HtmlSnippet = message.Snippet;

                await _uow.Save();
                await _dispatcher.Publish(new DeviceRecordStateChanged(record.Id, DmlOperation.Update));
                return CommandResult<HtmlSnippetState>.Success(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating/updating html snippet from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}