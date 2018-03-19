using Borg.Cms.Basic.Lib.System;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.ViewComponents
{
    public partial class ComponentSheduleViewComponent : TidingsViewComponentModule<Tidings>
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly ISentinel _sentinel;

        public ComponentSheduleViewComponent(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, ISentinel sentinel) : base(loggerFactory)
        {
            _uow = uow;
            _sentinel = sentinel;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            if (!tidings.ContainsKey(Tidings.DefinedKeys.Id))
            {
                _logger.Error(new ArgumentNullException("component id"), "{component} failed to render", nameof(ArticleDocumentAssociationsViewComponent));
                return null;
            }
            if (!int.TryParse(tidings[Tidings.DefinedKeys.Id], out int id))
            {
                _logger.Error(new ArgumentNullException("component id"), "{component} failed to render", nameof(ArticleDocumentAssociationsViewComponent));
                return null;
            }
            var sch = await _uow.QueryRepo<ComponentJobScheduleState>().Find(x => x.ComponentId == id,
                SortBuilder.Get<ComponentJobScheduleState>(x => x.ScheduleId).Build(), CancellationToken.None);

            var bucket = new List<(ComponentJobScheduleState row, JobData job, StateData state)>();
            foreach (var sc in sch)
            {
                var hit = await _sentinel.JobData(sc.ScheduleId.ToString());
                bucket.Add((sc, hit.job, hit.state));
            }

            var deletedrows = false;
            foreach (var valueTuple in bucket)
            {
                if (valueTuple.job == null && valueTuple.state == null)
                {
                    await _uow.ReadWriteRepo<ComponentJobScheduleState>().Delete(x =>
                         x.ComponentId == valueTuple.row.ComponentId && x.ScheduleId == valueTuple.row.ScheduleId);
                    deletedrows = true;
                }
            }
            if (deletedrows) await _uow.Save();

            var model = new ViewModels.ComponentSheduleViewModel { Records = bucket.ToArray(), ComponentId = id };
            return tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View]) ? View(tidings[Tidings.DefinedKeys.View], model) : View(model);
        }
    }
}