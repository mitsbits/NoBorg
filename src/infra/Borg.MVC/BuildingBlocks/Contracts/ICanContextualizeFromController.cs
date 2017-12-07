using Borg.MVC.Services.ServerResponses;
using Microsoft.AspNetCore.Mvc;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ICanContextualizeFromController : ICanContextualize
    {
        void Contextualize(Controller controller);
    }
}