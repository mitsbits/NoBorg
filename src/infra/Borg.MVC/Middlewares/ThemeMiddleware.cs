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

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}