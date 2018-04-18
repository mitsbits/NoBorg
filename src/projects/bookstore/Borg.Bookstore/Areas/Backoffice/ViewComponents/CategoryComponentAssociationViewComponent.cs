//using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components;
//using Borg.Infra.DAL;
//using Borg.Infra.DTO;
//using Borg.MVC.BuildingBlocks;
//using Borg.Platform.EF.CMS;
//using Borg.Platform.EF.CMS.Data;
//using Borg.Platform.EF.Contracts;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.ViewComponents
//{
//    public class CategoryComponentAssociationViewComponent : ViewComponentModule<Tidings>
//    {
//        private readonly IUnitOfWork<CmsDbContext> _uow;
//        private readonly ILogger _logger;

//        public CategoryComponentAssociationViewComponent(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
//        {
//            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
//            _uow = uow;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
//        {
//            if (!tidings.ContainsKey(Tidings.DefinedKeys.Id))
//            {
//                _logger.Error(new ArgumentNullException("component id"), "{component} failed to render", nameof(ArticleDocumentAssociationsViewComponent));
//                return null;
//            }
//            if (!int.TryParse(tidings[Tidings.DefinedKeys.Id], out int id))
//            {
//                _logger.Error(new ArgumentNullException("component id"), "{component} failed to render", nameof(ArticleDocumentAssociationsViewComponent));
//                return null;
//            }
//            var model = new CategoryComponentAssociationViewModel
//            {
//                ComponentId = id
//            };

//            var collection = await _uow.QueryRepo<CategoryComponentAssociationState>().Find(x => x.ComponentId == id,
//                SortBuilder.Get<CategoryComponentAssociationState>(x => x.ComponentId, false).Build());

//            model.PrimaryCategoryId = collection.Any(x => x.IsPrimary) ? collection.First(x => x.IsPrimary).CategoryId : default(int);
//            model.AssociatedCategories = collection.Select(x => x.CategoryId).ToArray();

//            model.CategoryGroups = await _uow.Context.CategoryGroupingStates.Include(x => x.Component)
//                .Include(x => x.Categories).ThenInclude(x => x.Component)
//                .Include(x => x.Categories).ThenInclude(x => x.Taxonomy)
//                .AsNoTracking().ToArrayAsync();

//            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], model);
//            return View(model);
//        }
//    }

//    public class CategoryComponentAssociationViewModel
//    {
//        public int ComponentId { get; set; }
//        public int PrimaryCategoryId { get; set; }
//        public int[] AssociatedCategories { get; set; }

//        public CategoryGroupingState[] CategoryGroups { get; set; }
//    }
//}