using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.ServerResponses;
namespace Borg.MVC.Services.UserSession
{
    public interface IContextAwareUserSession : IUserSession, ICanContextualize, ICanContextualizeFromController, ISessionServerResponseProvider, IUserSessionStorage
    {

    }
}