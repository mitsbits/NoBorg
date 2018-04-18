using Borg.Infra.DAL;
using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Platform.MediatR
{
    public abstract class CommandBase<TCommandResult> : MessageBase, IRequest<TCommandResult> where TCommandResult : CommandResult
    {
    }
}