using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductReviews.API.Middleware
{
    public class ExecutionTimeMiddleware
    {
        private readonly RequestDelegate _next;
        public ExecutionTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }   
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Append("X-Server-Name", Environment.MachineName);
            context.Response.Headers.Append("X-OS-Version", Environment.OSVersion.VersionString);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            context.Response.OnStarting(state => {
                var httpContext = (HttpContext)state;
                stopwatch.Stop();
                httpContext.Response.Headers.Append("X-Request-Execution-Time", stopwatch.ElapsedMilliseconds.ToString());
                return Task.CompletedTask;
            }, context);
            await _next(context);
        }
    }

    public static class ExecutionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExecutionTime(this IApplicationBuilder bld)
        {
            return bld.UseMiddleware<ExecutionTimeMiddleware>();
        }
    }
}