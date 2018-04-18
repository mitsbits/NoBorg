//using Borg.Infra.DAL;
//using Borg.Infra.DTO;
//using Borg.MVC.BuildingBlocks;
//using Borg.Platform.EF.CMS;
//using Borg.Platform.EF.CMS.Data;
//using Borg.Platform.EF.Contracts;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.ViewComponents
//{
//    public class ArticleTagsAssociationViewComponent : ViewComponentModule<Tidings>
//    {
//        private readonly IUnitOfWork<CmsDbContext> _uow;
//        private readonly ILogger _logger;

//        public ArticleTagsAssociationViewComponent(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
//        {
//            _logger = loggerFactory.CreateLogger(GetType());
//            _uow = uow;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
//        {
//            if (!tidings.ContainsKey(Tidings.DefinedKeys.Id))
//            {
//                _logger.Error(new ArgumentNullException("article id"), "{component} failed to render", nameof(ArticleTagsAssociationViewComponent));
//                return null;
//            }
//            if (!int.TryParse(tidings[Tidings.DefinedKeys.Id], out int id))
//            {
//                _logger.Error(new ArgumentNullException("article id"), "{component} failed to render", nameof(ArticleTagsAssociationViewComponent));
//                return null;
//            }

//            var tags = await _uow.QueryRepo<TagState>().Find(x => x.ArticleTags.Any(l => l.ArticleId == id), SortBuilder.Get<TagState>().Add(x => x.Tag).Build());

//            var model = new ArticleTagsAssociationViewModel() { ArticleId = id, Tags = tags, Selection = tags.Select(x => x.Tag).ToArray() };

//            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], model);
//            return View(model);
//        }
//    }

//    public class ArticleTagsAssociationViewModel
//    {
//        public int ArticleId { get; set; }
//        public IEnumerable<TagState> Tags { get; set; }

//        public string[] Selection { get; set; } = new string[0];
//    }
//}