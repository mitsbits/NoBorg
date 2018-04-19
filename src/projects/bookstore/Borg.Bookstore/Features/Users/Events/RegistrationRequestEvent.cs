using System;
using System.Net.Http;
using Borg.Infra.DDD;
using Borg.Infra.Messaging;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace Borg.Bookstore.Features.Users.Events
{
    public class RegistrationRequestEvent : MessageBase, INotification
    {
        public RegistrationRequestEvent(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }

    public class SendConfirmationCodeForRegistration : AsyncNotificationHandler<RegistrationRequestEvent>
    {
        //private readonly IEmailSender _sender;
        //private readonly IActionContextAccessor _accessor;
        //private readonly AtlasSettings _settings;
        //private readonly ILogger _logger;

        //public SendConfirmationCodeForRegistration(ILoggerFactory loggerFactory, IEmailSender sender, IActionContextAccessor accessor, AtlasSettings settings)
        //{
        //    _logger = loggerFactory.CreateLogger(GetType());
        //    _sender = sender;
        //    _accessor = accessor;
        //    _settings = settings;
        //}

        //public async Task Handle(RegistrationRequestEvent notification)
        //{
        //    try
        //    {
        //        IUrlHelper url = new UrlHelper(_accessor.ActionContext);
        //        var link = _settings.Tenant.ApplicationRoot + url.Page("/Emails/RegistartionConfirmation",
        //               new { pk = notification.Key.Partition, rk = notification.Key.Row });
        //        _logger.Debug("conf: {link}", link);
        //        //var link =
        //        //    $"http://localhost:2632/Emails/RegistartionConfirmation?pk={notification.Key.Partition}&rk={notification.Key.Row}";

        //        using (var client = new HttpClient())
        //        {
        //            var content = await client.GetStringAsync(link);
        //            _logger.Debug("conf: {email}", content);
        //            _sender.Send(EmailAccount.Create(notification.Key.Partition), "Registration verification", content);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, "{function} encoubtered {@exception}", nameof(SendConfirmationCodeForRegistration), ex);
        //        throw;
        //    }
        //}

        protected override async Task HandleCore(RegistrationRequestEvent notification)
        {
            throw new NotImplementedException(); //TODO : fix this
        }
    }
}