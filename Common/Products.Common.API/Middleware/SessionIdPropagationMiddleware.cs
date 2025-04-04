using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Products.Common.API.Middleware
{
    public class SessionIdPropagationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionIdPropagationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();

            var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Session-Id", traceId);
            await _next(context);
        }
    }

}
