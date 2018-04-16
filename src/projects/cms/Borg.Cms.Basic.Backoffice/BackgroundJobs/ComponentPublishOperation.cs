using Borg.Infra.DAL;
using Borg.Infra.Services.BackgroundServices;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.BackgroundJobs
{
    public class ComponentPublishOperation
    {
        public ComponentPublishOperation(int componentId, OperationDirection direction)
        {
            ComponentId = componentId;
            Direction = direction;
        }

        public int ComponentId { get; }
        public OperationDirection Direction { get; }

        public enum OperationDirection { Up, Down }

        public string[] JobArgs() => new[] { ComponentId.ToString(), Direction.ToString() };

        public static ComponentPublishOperation FromArgs(string[] args) => new ComponentPublishOperation(int.Parse(args[0]), args[1].ToLower() == "up" ? OperationDirection.Up : OperationDirection.Down);
    }

    public class ComponentPublishStateJob : IEnqueueJob
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public ComponentPublishStateJob(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _uow = uow;
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public async Task Execute(string[] args)
        {
            var ops = ComponentPublishOperation.FromArgs(args);
            var comp = await _uow.ReadWriteRepo<ComponentState>().Get(x => x.Id == ops.ComponentId);
            if (ops.Direction != ComponentPublishOperation.OperationDirection.Down)
            {
                comp.Publish();
            }
            else
            {
                comp.Suspend();
            }
            await _uow.ReadWriteRepo<ComponentState>().Update(comp);
            await _uow.Save();
        }
    }
}