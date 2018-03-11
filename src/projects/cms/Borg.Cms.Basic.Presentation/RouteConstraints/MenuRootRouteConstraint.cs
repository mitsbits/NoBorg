using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using Borg.Infra;
using Borg.MVC.PlugIns.Decoration;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Cms.Basic.Presentation.RouteConstraints
{
    [PlugInRouteConstraint("Menu root constraint")]
    public class MenuRootRouteConstraint : IRouteConstraint
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly List<(int id, string slug)> _bucket = new List<(int id, string slug)>();
        private readonly IMediator _dispatcher;
        public const string ROUTE_IDENTIFIER = "rootmenu";

        public MenuRootRouteConstraint(IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            Populate();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
        
            return _bucket.Any(x => x.slug == values[routeKey]?.ToString().ToLowerInvariant());
        }

        private void Populate()
        {
            var roots = _uow.Context.NavigationItemStates.AsNoTracking()
                .Where(x => x.Taxonomy.ParentId == 0);
            var q = from r in roots
                    join l in _uow.Context.NavigationItemStates.AsNoTracking() on r.Id equals l.Taxonomy.ParentId into rls
                    from l in rls.DefaultIfEmpty()
                    where l == null
                    select new { r.Id, r.Path };

            foreach (var x in q)
            {
                var page = AsyncHelpers.RunSync(() => _dispatcher.Send(new ComponentPageContentRequest(x.Id)));
                var device = AsyncHelpers.RunSync(() => _dispatcher.Send(new ComponentDeviceRequest(x.Id)));
                _bucket.Add((id: x.Id, slug: x.Path.TrimStart('/').TrimEnd('/').ToLowerInvariant()));
            }
        }
    }

    [PlugInRouteConstraint("Menu parent leaf constraint")]
    public class MenuLeafParentRouteConstraint : Microsoft.AspNetCore.Routing.IRouteConstraint
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly List<string> _bucket = new List<string>();
        public const string ROUTE_IDENTIFIER = "parentmenu";

        public MenuLeafParentRouteConstraint(IUnitOfWork<CmsDbContext> uow)
        {
            _uow = uow;
            Populate();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return _bucket.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }

        private void Populate()
        {
            var roots = _uow.Context.NavigationItemStates.AsNoTracking();
            var q = from r in roots
                    join l in _uow.Context.NavigationItemStates.AsNoTracking() on r.Id equals l.Taxonomy.ParentId into rls
                    from l in rls.DefaultIfEmpty()
                    where l != null
                    select r.Path;

            _bucket.AddRange(q.ToList().Select(x => x.TrimStart('/').TrimEnd('/').ToLowerInvariant()));
        }
    }

    [PlugInRouteConstraint("Menu child leaf constraint")]
    public class MenuLeafChildRouteConstraint : Microsoft.AspNetCore.Routing.IRouteConstraint
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly List<string> _bucket = new List<string>();
        public const string ROUTE_IDENTIFIER = "childmenu";

        public MenuLeafChildRouteConstraint(IUnitOfWork<CmsDbContext> uow)
        {
            _uow = uow;
            Populate();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return _bucket.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }

        private void Populate()
        {
            var roots = _uow.Context.NavigationItemStates.AsNoTracking();
            var q = from r in roots
                    join l in _uow.Context.NavigationItemStates.AsNoTracking() on r.Id equals l.Taxonomy.ParentId into rls
                    from l in rls.DefaultIfEmpty()
                    where l != null
                    select l.Path;

            _bucket.AddRange(q.ToList().Select(x => x.TrimStart('/').TrimEnd('/').ToLowerInvariant()));
        }
    }
}