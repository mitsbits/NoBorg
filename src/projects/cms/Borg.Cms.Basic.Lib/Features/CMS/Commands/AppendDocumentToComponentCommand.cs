using Borg.Infra.DAL;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class AppendDocumentToComponentCommand : CommandBase<CommandResult>
    {
        public int RecordId { get; set; }
        public IFormFile File { get; set; }

        [Required]
        public string Email { get; set; }
    }
}