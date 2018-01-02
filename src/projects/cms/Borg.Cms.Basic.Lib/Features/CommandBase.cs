using Borg.Infra.DAL;
using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features
{
    public class CommandBase<TCommandResult> : MessageBase, IRequest<TCommandResult> where TCommandResult : CommandResult
    {
    }
}