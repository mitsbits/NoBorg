using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Documents;
using Borg.Platform.MediatR;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Borg.Infra.Storage.Contracts;

namespace Borg.Bookstore.Features.Users.Commands
{
    public class UserAvatarCommand : CommandBase<CommandResult>
    {
        public UserAvatarCommand()
        {
        }

        public UserAvatarCommand(string email, string claimType, string claimValue = "")
        {
            Email = email;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public IFormFile File { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Claim Type")]
        public string ClaimType { get; set; }

        [Required]
        [DisplayName("Claim Value")]
        public string ClaimValue { get; set; }
    }

    public class UserAvatarCommandHandler : AsyncRequestHandler<UserAvatarCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IStaticImageCacheStore<int> _cacheStore;
        private readonly IDocumentsService<int> _documents;

        public UserAvatarCommandHandler(ILoggerFactory loggerFactory, IMediator dispatcher, IStaticImageCacheStore<int> cacheStore, IDocumentsService<int> documents)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _dispatcher = dispatcher;
            _cacheStore = cacheStore;
            _documents = documents;
            _documents.FileCreated += DocumentsOnFileCreated;
        }

        private Task DocumentsOnFileCreated(IFileSpec<int> file)
        {
            return _cacheStore.PrepareSizes(file.Id);
        }

        protected override async Task<CommandResult> HandleCore(UserAvatarCommand message)
        {
            try
            {
                var onlysetclaim = message.File == null;
                if (onlysetclaim)
                {
                    return await _dispatcher.Send(new UserClaimCommand(message.Email, message.ClaimType,
                        message.ClaimValue));
                }

                string filename = ContentDispositionHeaderValue.Parse(message.File.ContentDisposition).FileName.ToString().Trim('"');
                filename = filename.EnsureCorrectFilenameFromUpload();
                using (var stream = new MemoryStream())
                {
                    await message.File.CopyToAsync(stream);
                    stream.Seek(0, 0);
                    var docId = await _documents.StoreUserDocument(stream.ToArray(), filename, message.Email);
                    var avatarUrl = await _cacheStore.PublicUrl(docId.file.Id, VisualSize.Medium);
                    return await _dispatcher.Send(new UserClaimCommand(message.Email, message.ClaimType, avatarUrl.AbsoluteUri));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error updating claim for {user}: @exception", message.Email, ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}