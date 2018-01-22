using Borg;
using Borg.Infra.Messaging;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR
{
    [DebuggerStepThrough]
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _log;

        public LoggingBehavior(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger(GetType());
        }

        [DebuggerStepThrough]
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _log.Debug("Handling {request} with parameter {@request}", request.GetType().Name, request);
            var watch = Stopwatch.StartNew();
            var response = await next();
            watch.Stop();
            _log.Debug("Handled {request} to {@response} - {elapsed}", request.GetType().Name, response, watch.Elapsed);

            return response;
        }
    }

    [DebuggerStepThrough]
    public class CorrelationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        [DebuggerStepThrough]
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var response = await next();
            if (!(request is ICorrelated correlated)) return response;
            var correlation = response as ICorrelatedResponse;
            correlation?.Corralate(correlated);
            return response;
        }
    }
}