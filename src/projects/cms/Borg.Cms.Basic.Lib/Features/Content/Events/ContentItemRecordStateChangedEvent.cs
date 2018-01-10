using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Infra.DAL;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.Content.Events
{
    public class ContentItemRecordStateChangedEvent : EntityRecordStateChanged<ContentItemRecord>, INotification
    {
        public ContentItemRecordStateChangedEvent(int id, DmlOperation dmlOperation) : base(id, dmlOperation)
        {
        }
    }
}