namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IDocumentBehaviour
    {
        DocumentBehaviourState DocumentBehaviourState { get; }
    }

    public enum DocumentBehaviourState
    {
        Commited = 0,
        InProgress = 1
    }
}