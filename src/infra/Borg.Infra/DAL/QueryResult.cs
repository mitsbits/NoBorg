using System;
using Borg.Infra.Messaging;

namespace Borg.Infra.DAL
{
    public class QueryResult : ICorrelatedResponse
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

        public string[] Errors { get; protected set; } = new string[0];

        public static QueryResult Failure(params string[] errors)
        {
            return new QueryResult { Outcome = TransactionOutcome.Failure, Errors = errors };
        }
        private Guid _correlationId = Guid.Empty;
        public Guid CorrelationId => _correlationId;

        public void Corralate(ICorrelated message)
        {
            _correlationId = message.CorrelationId;
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

        public static QueryResult<T> Success(T payload)
        {
            return new QueryResult<T>(TransactionOutcome.Success, payload);
        }

        public new static QueryResult<T> Failure(params string[] errors)
        {
            return new QueryResult<T>(TransactionOutcome.Failure) { Errors = errors };
        }
    }
}