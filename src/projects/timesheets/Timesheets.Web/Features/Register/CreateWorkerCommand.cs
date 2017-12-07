using Borg.Infra.DAL;
using MediatR;
using Timesheets.Web.Domain.Infrastructure;

namespace Timesheets.Web.Features.Register
{
    public class CreateWorkerCommand : IRequest<CommandResult>
    {
        public CreateWorkerCommand(RegisterViewModel model)
        {
            Model = model;
        }

        public RegisterViewModel Model { get; }
    }
}
