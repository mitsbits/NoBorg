namespace Borg.Infra.DAL
{
    public enum TransactionOutcome
    {
        Success = 1,
        Failure = -1
    }

    public enum DmlOperation
    {
        Create,
        Update,
        Delete
    }
}