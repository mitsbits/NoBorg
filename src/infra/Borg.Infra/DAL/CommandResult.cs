namespace Borg.Infra.DAL
{
    public class CommandResult
    {
        protected CommandResult()
        {
        }

        protected CommandResult(TransactionOutcome outcome) : this()
        {
            Outcome = outcome;
        }

        public bool Succeded => Outcome == TransactionOutcome.Success;

        public TransactionOutcome Outcome { get; protected set; }

        public string[] Errors { get; protected set; }

        public static CommandResult Success()
        {
            return new CommandResult { Outcome = TransactionOutcome.Success };
        }

        public static CommandResult Failure(params string[] errors)
        {
            return new CommandResult { Outcome = TransactionOutcome.Failure, Errors = errors };
        }
    }

    public class CommandResult<T> : CommandResult
    {
        protected CommandResult(TransactionOutcome outcome) : base(outcome)
        {
            Outcome = outcome;
        }

        public T Payload { get; private set; }

        public static CommandResult<T> Success(T payload)
        {
            return new CommandResult<T>(TransactionOutcome.Success) { Payload = payload };
        }

        public static CommandResult<T> FailureWithEmptyPayload(params string[] errors) => new CommandResult<T>(TransactionOutcome.Failure) { Errors = errors };
    }
}