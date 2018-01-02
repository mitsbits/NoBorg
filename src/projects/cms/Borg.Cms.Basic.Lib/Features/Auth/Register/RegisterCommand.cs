using Borg.Infra.DAL;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.Auth.Register
{
    public class RegisterCommand : CommandBase<CommandResult>, IRequest<CommandResult>
    {
        public RegisterCommand(RegisterViewModel model)
        {
            Model = model;
        }

        public RegisterViewModel Model { get; }
    }
}