using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Infra.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Auth.Management.Users
{
    public class UserViewModel : UserRowViewModel
    {
        public IdentityUserClaim<string>[] UserClaims { get; set; }
    }

    public class UserRequest : IRequest<QueryResult<UserViewModel>>
    {
        public UserRequest(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }

    public class UserRequestHandler : AsyncRequestHandler<UserRequest, QueryResult<UserViewModel>>
    {
        private readonly AuthDbContext _db;
        private readonly ILogger _log;

        public UserRequestHandler(ILoggerFactory loggerFactory, AuthDbContext db)
        {
            _log = loggerFactory.CreateLogger(GetType());
            _db = db;
        }

        protected override async Task<QueryResult<UserViewModel>> HandleCore(UserRequest message)
        {
            try
            {
                var query = from u in _db.Users
                            join c in _db.UserClaims on u.Id equals c.UserId
                            join ur in _db.UserRoles on u.Id equals ur.UserId into urx
                            from ux in urx.DefaultIfEmpty()
                            join r in _db.Roles on ux.RoleId equals r.Id into rx
                            from x in rx.DefaultIfEmpty()
                            where u.Email.ToLower() == message.Email.ToLower()
                            select new { U = u, C = c, R = x };

                var hit = await query.ToListAsync();

                if (hit == null || !hit.Any())
                    return QueryResult<UserViewModel>.Failure($"User with email {message.Email} was not found");

                var model = new UserViewModel()
                {
                    LockedOut = hit.First().U.LockoutEnabled,
                    LockedOutEnd = hit.First().U.LockoutEnd,
                    UserName = hit.First().U.UserName,
                    UserId = hit.First().U.Id,
                    FirstName = hit.FirstOrDefault(x => x.C.ClaimType == ClaimTypes.GivenName)?.C.ClaimValue,
                    LastName = hit.FirstOrDefault(x => x.C.ClaimType == ClaimTypes.Surname)?.C.ClaimValue,
                    Roles = hit.Any(x => x.R != null) ? hit.Select(x => x.R.Name).Distinct().ToArray() : new string[0],
                    UserClaims = hit.Any(x => x.C != null) ? hit.Select(x => x.C).ToArray() : new IdentityUserClaim<string>[0],
                };

                return QueryResult<UserViewModel>.Success(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return QueryResult<UserViewModel>.Failure(ex.Message);
            }
        }
    }
}