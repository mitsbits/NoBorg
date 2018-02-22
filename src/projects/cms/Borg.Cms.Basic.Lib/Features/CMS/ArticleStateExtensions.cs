using Borg.Cms.Basic.Lib.Features.CMS.Commands;
using Borg.Platform.EF.CMS;

namespace Borg
{
    public static class ArticleStateExtensions
    {
        public static ArticlePrimaryImageCommand ArticlePrimaryImageCommand(this ArticleState state, string userHandle)
        {
            var command = new ArticlePrimaryImageCommand()
            {
                RecordId = state.Id,
                RemoveOperation = false,
                UserHandle = userHandle,
                ExistingFile = state.PageMetadata?.PrimaryImageFileId
            };
            return command;
        }
    }
}