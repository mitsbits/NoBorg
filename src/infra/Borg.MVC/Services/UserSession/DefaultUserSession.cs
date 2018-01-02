using Borg.Infra;
using Microsoft.AspNetCore.Http;

namespace Borg.MVC.Services.UserSession
{
    public class DefaultUserSession : UserSession
    {
        public DefaultUserSession(IHttpContextAccessor httpContextAccessor, ISerializer serializer) : base(httpContextAccessor, serializer)
        {
        }

        public override bool ContextAcquired { get; protected set; } = true;
    }
}