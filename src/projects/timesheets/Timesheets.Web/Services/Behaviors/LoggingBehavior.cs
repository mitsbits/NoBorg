using Borg;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Timesheets.Web.Services.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _log;

        public LoggingBehavior(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger(GetType());
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            _log.Debug("Handling {request} with parameter {@request}", request.GetType().Name, request);
            var watch = Stopwatch.StartNew();
            var response = await next();
            watch.Stop();
            _log.Debug("Handled {request} to {@response} - {elapsed}", request.GetType().Name, response, watch.Elapsed);

            return response;
        }
    }
}