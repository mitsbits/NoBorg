using Borg.Infra.Messaging;
using MediatR;
using System.Threading.Tasks;

namespace Timesheets.Web.Services.Behaviors
{
    public class CorrelationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            if (!(request is ICorrelated correlated)) return response;
            var correlation = response as ICorrelatedResponse;
            correlation?.Corralate(correlated);
            return response;
        }
    }
}