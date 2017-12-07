namespace Borg.Infra.DAL
{
    public class QueryResult
    {
        protected QueryResult()
        {
        }

        protected QueryResult(TransactionOutcome outcome) : this()
        {
            Outcome = outcome;
        }

        public bool Succeded => Outcome == TransactionOutcome.Success;

        public TransactionOutcome Outcome { get; protected set; }

        public string[] Errors { get; private set; } = new string[0];

        public static QueryResult Failure(params string[] errors)
        {
            return new QueryResult { Outcome = TransactionOutcome.Failure, Errors = errors };
        }
    }

    public class QueryResult<T> : QueryResult
    {
        private readonly T _payload;

        protected QueryResult(TransactionOutcome outcome) : base(outcome)
        {
            Outcome = outcome;
        }

        protected QueryResult(TransactionOutcome outcome, T payload) : base(outcome)
        {
            Outcome = outcome;
            _payload = payload;
        }

        public T Payload => Succeded ? _payload : default(T);

        public static QueryResult Success(T payload)
        {
            return new QueryResult<T>(TransactionOutcome.Success, payload);
        }
    }
}