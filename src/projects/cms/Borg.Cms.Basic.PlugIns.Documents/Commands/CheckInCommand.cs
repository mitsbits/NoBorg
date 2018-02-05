using Borg.Cms.Basic.Lib.Features;
using Borg.Infra.DAL;
using Microsoft.AspNetCore.Http;

namespace Borg.Cms.Basic.PlugIns.Documents.Commands
{
    public class CheckInCommand : CommandBase<CommandResult>
    {
        public CheckInCommand() { }

        public CheckInCommand(int documentId, string userHandle)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
        }

        public int DocumentId { get; set; }
        public string UserHandle { get; set; }
        public IFormFile File { get; set; }
    }
}