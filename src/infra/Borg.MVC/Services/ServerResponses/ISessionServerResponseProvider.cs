using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.Services.ServerResponses
{
    public interface ISessionServerResponseProvider : IServerResponseProvider<ServerResponse>, ICanContextualize
    {
    }
}