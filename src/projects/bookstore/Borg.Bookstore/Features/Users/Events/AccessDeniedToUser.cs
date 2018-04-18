using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Bookstore.Features.Users.Events
{
    public class AccessDeniedToUser : MessageBase, INotification
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