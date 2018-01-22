using Borg.Infra.DAL;
using Borg.Infra.DDD;
using Borg.Infra.DDD.Contracts;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.Device.Events
{
    public abstract class EntityRecordStateChanged<TEntity> : EntityStateChangedEvent<TEntity, int> where TEntity : IEntity<int>
    {
        protected EntityRecordStateChanged(int id, DmlOperation dmlOperation) : base(id, dmlOperation)
        {
        }
    }

    public class DeviceRecordStateChanged : EntityRecordStateChanged<DeviceRecord>, INotification
    {
        public DeviceRecordStateChanged(int id, DmlOperation dmlOperation) : base(id, dmlOperation)
        {
        }
    }

    public class SectionRecordStateChanged : EntityRecordStateChanged<SectionRecord>, INotification
    {
        public SectionRecordStateChanged(int id, DmlOperation dmlOperation) : base(id, dmlOperation)
        {
        }
    }

    public class SlotRecordStateChanged : EntityRecordStateChanged<SlotRecord>, INotification
    {
        public SlotRecordStateChanged(int id, DmlOperation dmlOperation) : base(id, dmlOperation)
        {
        }
    }
}