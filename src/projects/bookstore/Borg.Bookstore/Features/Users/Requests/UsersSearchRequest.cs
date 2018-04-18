using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Borg.Bookstore.Features.Users.ViewModels;
using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Platform.Identity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Borg.Bookstore.Features.Users.Requests
{
    public class UsersSearchRequest : IRequest<QueryResult<IPagedResult<UserRowViewModel>>>
    {
        public UsersSearchRequest(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public string SearchTerm { get; }
    }

    public class UsersSearchRequestHandler : AsyncRequestHandler<UsersSearchRequest, QueryResult<IPagedResult<UserRowViewModel>>>
    {
        private readonly AuthDbContext _db;

        public UsersSearchRequestHandler(AuthDbContext db)
        {
            _db = db;
        }

        protected override async Task<QueryResult<IPagedResult<UserRowViewModel>>> HandleCore(UsersSearchRequest message)
        {
            var s = message.SearchTerm.Trim();

            var ids = from u in _db.Users
                      join c in _db.UserClaims on u.Id equals c.UserId
                      where string.IsNullOrWhiteSpace(s) || u.Email.Contains(s) || c.ClaimType.EndsWith("Name") && c.ClaimValue.Contains(s)
                      select new { u.Id };

            var query = from u in _db.Users
                        join f in ids on u.Id equals f.Id
                        join c in _db.UserClaims on u.Id equals c.UserId
                        join ur in _db.UserRoles on u.Id equals ur.UserId into urx
                        from ux in urx.DefaultIfEmpty()
                        join r in _db.Roles on ux.RoleId equals r.Id into rx
                        from x in rx.DefaultIfEmpty()
                        select new { U = u, C = c, R = x };

            var data = await query.ToListAsync();

            var groups = data.GroupBy(x => x.U.Id);

            var models = groups.Select(@group => new UserRowViewModel
            {
                LockedOut = @group.First().U.LockoutEnabled,
                LockedOutEnd = @group.First().U.LockoutEnd,
                UserName = @group.First().U.UserName,
                UserId = @group.Key,
                FirstName = @group.FirstOrDefault(x => x.C.ClaimType == ClaimTypes.GivenName)?.C.ClaimValue,
                LastName = @group.FirstOrDefault(x => x.C.ClaimType == ClaimTypes.Surname)?.C.ClaimValue,
                Roles = data.Where(x => x.U.Id == @group.Key && x.R != null).Select(x => x.R.Name).Distinct().ToArray()
            }).ToList();

            var count = models.Count;
            IPagedResult<UserRowViewModel> result = new PagedResult<UserRowViewModel>(models, 1, count, count);
            return QueryResult<IPagedResult<UserRowViewModel>>.Success(result);
        }
    }
}