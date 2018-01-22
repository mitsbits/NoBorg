using Borg.Infra.DAL;

namespace Borg.Cms.Basic.Lib.Features
{
    public abstract class UserCommandBase<TCommandResult> : CommandBase<TCommandResult> where TCommandResult : CommandResult
    {
        protected UserCommandBase(string userHandle)
        {
            UserHandle = userHandle;
        }

        public string UserHandle { get; }
    }
}