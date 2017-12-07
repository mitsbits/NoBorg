namespace Borg.Infra.Messaging
{
    public interface ITopicPublisher : IMessagePublisher
    {
        string Topic { get; }
    }
}