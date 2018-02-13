using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var model = new ArticlePageMetadataViewModel();
            model.HtmlMetas = new JsonEditor(JsonConvert.SerializeObject(collection));
            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], model);
            return View(model);
        }
    }

    public class ArticlePageMetadataViewModel
    {
        public JsonEditor HtmlMetas { get; set; }
    }
}
