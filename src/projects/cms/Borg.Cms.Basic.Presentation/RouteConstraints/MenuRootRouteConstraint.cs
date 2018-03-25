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
using Borg.Cms.Basic.Presentation.Services.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.Cms.Basic.Presentation.RouteConstraints
{
    [PlugInRouteConstraint("Menu root constraint")]
    public class MenuRootRouteConstraint : IRouteConstraint
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly List<(int id, string slug, IPageContent page, IDeviceStructureInfo device)> _bucket = new List<(int id, string slug, IPageContent page, IDeviceStructureInfo device)>();
        private readonly IMediator _dispatcher;
        private readonly IEntityMemoryStore _memoryStore;
        public const string ROUTE_IDENTIFIER = "rootmenu";

        public MenuRootRouteConstraint(IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, IEntityMemoryStore memoryStore)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _memoryStore = memoryStore;
            Populate();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
        var hit = _bucket.FirstOrDefault(x => x.slug == values[routeKey]?.ToString().ToLowerInvariant());
            var ismatch = hit.id > 0;
            if (ismatch)
            {
                values["component"] = new ComponentPageDescriptor<int>()
                {
                    ComponentId = hit.id,
                    PageContent = hit.page,
                    Device = hit.device,
                    Slug = hit.slug
                };
    
            }
            return ismatch;
        }

        private void Populate()
        {
            var roots = _memoryStore.NavigationItems.Where(x => x.Taxonomy.ParentId == 0);
            var q = from r in roots
                    join l in _memoryStore.NavigationItems on r.Id equals l.Taxonomy.ParentId into rls
                    from l in rls.DefaultIfEmpty()
                    where l == null
                    select new { r.Id, r.Path };

            foreach (var x in q)
            {
                var page = AsyncHelpers.RunSync(() => _dispatcher.Send(new ComponentPageContentRequest(x.Id)));
                var device = AsyncHelpers.RunSync(() => _dispatcher.Send(new ComponentDeviceRequest(x.Id)));
                var brick = (id: x.Id, slug: x.Path.TrimStart('/').TrimEnd('/').ToLowerInvariant(), page.Payload.content, device.Payload);
                _bucket.Add(brick);
            }
        }
    }

    [PlugInRouteConstraint("Menu parent leaf constraint")]
    public class MenuLeafParentRouteConstraint : Microsoft.AspNetCore.Routing.IRouteConstraint
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly List<string> _bucket = new List<string>();
        public const string ROUTE_IDENTIFIER = "parentmenu";
        private readonly IEntityMemoryStore _memoryStore;
        public MenuLeafParentRouteConstraint(IUnitOfWork<CmsDbContext> uow, IEntityMemoryStore memoryStore)
        {
            _uow = uow;
            _memoryStore = memoryStore;
            Populate();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return _bucket.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }

        private void Populate()
        {
            var roots = _memoryStore.NavigationItems;
            var q = from r in roots
                    join l in _memoryStore.NavigationItems on r.Id equals l.Taxonomy.ParentId into rls
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
        private readonly List<(int id, string slug, IPageContent page, IDeviceStructureInfo device)> _bucket = new List<(int id, string slug, IPageContent page, IDeviceStructureInfo device)>();
        public const string ROUTE_IDENTIFIER = "childmenu";
        private readonly IMediator _dispatcher;
        private readonly IEntityMemoryStore _memoryStore;

        public MenuLeafChildRouteConstraint(IUnitOfWork<CmsDbContext> uow, IEntityMemoryStore memoryStore, IMediator dispatcher)
        {
            _uow = uow;
            _memoryStore = memoryStore;
            _dispatcher = dispatcher;
            Populate();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var hit = _bucket.FirstOrDefault(x => x.slug == values[routeKey]?.ToString().ToLowerInvariant());
            var ismatch = hit.id > 0;
            if (ismatch)
            {
                values["component"] = new ComponentPageDescriptor<int>()
                {
                    ComponentId = hit.id,
                    PageContent = hit.page,
                    Device = hit.device,
                    Slug = hit.slug
                };

            }
            return ismatch;
        }

        private void Populate()
        {
            var roots = _memoryStore.NavigationItems;
            var q = from r in roots
                    join l in _memoryStore.NavigationItems on r.Id equals l.Taxonomy.ParentId into rls
                    from l in rls.DefaultIfEmpty()
                    where l != null
                    select new{l.Id, l.Path};

            foreach (var x in q)
            {
                var page = AsyncHelpers.RunSync(() => _dispatcher.Send(new ComponentPageContentRequest(x.Id)));
                var device = AsyncHelpers.RunSync(() => _dispatcher.Send(new ComponentDeviceRequest(x.Id)));
                var brick = (id: x.Id, slug: x.Path.TrimStart('/').TrimEnd('/').ToLowerInvariant(), page.Payload.content, device.Payload);
                _bucket.Add(brick);
            }

        }
    }
}