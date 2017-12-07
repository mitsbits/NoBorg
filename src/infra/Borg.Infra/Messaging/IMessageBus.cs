namespace Borg.Infra.Messaging
{
    public interface IMessageBus : IMessagePublisher, IMessageSubscriber, IDispatcherInstance
    {
    }
}