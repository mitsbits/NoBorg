using Borg.CMS.Components;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Services.Editors;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components
{
    public class ArticlePageMetadataViewComponent : ViewComponentModule<Tidings>
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly ILogger _logger;

        public ArticlePageMetadataViewComponent(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            if (!tidings.ContainsKey(Tidings.DefinedKeys.Id))
            {
                _logger.Error(new ArgumentNullException("article id"), "{component} failed to render", nameof(ArticlePageMetadataViewComponent));
                return null;
            }
            if (!int.TryParse(tidings[Tidings.DefinedKeys.Id], out int id))
            {
                _logger.Error(new ArgumentNullException("article id"), "{component} failed to render", nameof(ArticlePageMetadataViewComponent));
                return null;
            }

            var hit = await _uow.QueryRepo<PageMetadataState>().Get(x => x.Id == id);
            if (hit == null) hit = new PageMetadataState();

            var collection = new HtmlMetaSet();
            try
            {
                collection = JsonConvert.DeserializeObject<HtmlMetaSet>(hit.HtmlMetaJsonText);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            var model = new ArticlePageMetadataViewModel
            {
                HtmlMetas = new JsonEditor(JsonConvert.SerializeObject(collection)),
                RecordId = id
            };
            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], model);
            return View(model);
        }
    }

    public class ArticlePageMetadataViewModel
    {
        public int RecordId { get; set; }
        public JsonEditor HtmlMetas { get; set; }
    }
}