using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.Auth.Events
{
    public class AccessDeniedToUser : TimestampedEvent,INotification
    {
        public AccessDeniedToUser(string userName, string returnUrl)
        {
            UserName = userName;
            ReturnUrl = returnUrl;
        }

        public string UserName { get; }
        public string ReturnUrl { get; }
    }
}