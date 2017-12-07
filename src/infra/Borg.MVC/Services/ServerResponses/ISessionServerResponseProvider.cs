using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;

namespace Borg.MVC.Services.ServerResponses
{
    public interface ISessionServerResponseProvider : IServerResponseProvider<ServerResponse>, ICanContextualize
    {
    }
}