using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Borg.MVC.Middlewares
{
    public class ThemeMiddleware
    {
        private readonly string _theme;

        private readonly RequestDelegate _next;

        public ThemeMiddleware(RequestDelegate next, string theme)
        {
            _next = next;
            _theme = theme;
        }

        public Task Invoke(HttpContext context)
        {
            context.Request.HttpContext.Items["theme"] = _theme;
            return _next(context);
        }
    }

    public class RouteMiddleware
    {
        private readonly RequestDelegate _next;

        public RouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            return _next(context);
        }
    }
}