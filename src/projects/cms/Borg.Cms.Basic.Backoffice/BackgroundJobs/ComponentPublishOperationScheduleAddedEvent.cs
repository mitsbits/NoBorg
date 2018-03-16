using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Backoffice.BackgroundJobs
{
    public class ComponentPublishOperationScheduleAddedEvent : MessageBase, INotification
    {
        public ComponentPublishOperationScheduleAddedEvent(int componentId, string jobHandle)
        {
            ComponentId = componentId;
            JobHandle = jobHandle;
        }

        public int ComponentId { get; }
        public string JobHandle { get; }
    }
}