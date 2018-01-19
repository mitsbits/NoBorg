using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Infra.DAL;
using Borg.Infra.DDD;
using Borg.Infra.Services;
using Borg.MVC;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra;

namespace Borg.Cms.Basic.Lib.Features.Auth.Register
{
    public class RegistrationRequestCommand : CommandBase<CommandResult<RegistrationRequest>>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => CompositeKey.Row;
            set => CompositeKey = CompositeKey<string>.Create(CompositeKey.Partition, value);
        }

        public CompositeKey<string> CompositeKey { get; set; } = CompositeKey<string>.Create(string.Empty, Randomize.String(7));

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email
        {
            get => CompositeKey.Partition;
            set => CompositeKey = CompositeKey<string>.Create(value, CompositeKey.Row);
        }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegistrationRequestCommandHandler : AsyncRequestHandler<RegistrationRequestCommand, CommandResult<RegistrationRequest>>
    {
        private readonly ILogger _logger;
        private readonly UserManager<CmsUser> _manager;
        private readonly IUnitOfWork<AuthDbContext> _uow;
        private readonly BorgSettings _settings;

        public RegistrationRequestCommandHandler(ILoggerFactory loggerFactory, UserManager<CmsUser> manager, IUnitOfWork<AuthDbContext> uow, BorgSettings settings)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _manager = manager;
            _uow = uow;
            _settings = settings;
        }

        protected override async Task<CommandResult<RegistrationRequest>> HandleCore(RegistrationRequestCommand message)
        {
            try
            {
                if (!_settings.Auth.ActivateOnRegisterRequest)
                {
                    var existingUser = await _manager.FindByEmailAsync(message.Email);
                    if (existingUser != null)
                    {
                        return CommandResult<RegistrationRequest>.FailureWithEmptyPayload(
                            $"User {message.Email.ToLower()} exists");
                    }

                    var repo = _uow.ReadWriteRepo<RegistrationRequest>();

                    var existingRequests = await repo.Find(x => x.Email == message.Email && x.Id == message.Id, null,
                        CancellationToken.None);
                    var registrationRequests =
                        existingRequests.Records as RegistrationRequest[] ?? existingRequests.ToArray();
                    if (registrationRequests.Any())
                    {
                        if (registrationRequests.Count() > 1)
                        {
                            var todelete = registrationRequests.OrderByDescending(x => x.SubmitedOn).Skip(1).ToList();
                            foreach (var registrationRequest in todelete)
                            {
                                await repo.Delete(x =>
                                    x.Id == registrationRequest.Id && x.Email == registrationRequest.Email);
                            }
                        }
                        var hit = registrationRequests.OrderByDescending(x => x.SubmitedOn).First();
                        return CommandResult<RegistrationRequest>.Success(hit);
                    }

                    var request = new RegistrationRequest()
                    {
                        Email = message.Email.ToLower(),
                        SubmitedOn = DateTimeOffset.UtcNow
                    };
                    await repo.Create(request);
                    await _uow.Save();

                    var user = await CreateUser(message);

                    return CommandResult<RegistrationRequest>.Success(request);
                }
                else
                {
                    var user = await CreateUser(message);
                    var activatetionResult = await _manager.SetLockoutEnabledAsync(user, false);
                    if (!activatetionResult.Succeeded) CommandResult<RegistrationRequest>.FailureWithEmptyPayload(activatetionResult.Errors.Select(x => x.Description).ToArray());
                    var outcome = new RegistrationRequest() { Email = user.Email };
                    return CommandResult<RegistrationRequest>.Success(outcome);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error requesting registration for {user} -  {@exception}", message.Email, ex);
                return CommandResult<RegistrationRequest>.FailureWithEmptyPayload(ex.ToString());
            }
        }

        private async Task<CmsUser> CreateUser(RegistrationRequestCommand message)
        {
            var user = new CmsUser() { UserName = message.Email, Email = message.Email };
            var result = await _manager.CreateAsync(user, message.Password);
            if (!result.Succeeded) return user;
            var results = new List<IdentityResult>
            {
                await _manager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, message.LastName)),
                await _manager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, message.FirstName))
            };
            if (results.Any(x => !x.Succeeded))
            {
                await _manager.DeleteAsync(user);
                _logger.LogDebug("deleting {@user}", message.Email);
                return user;
            }
            return user;
        }
    }
}