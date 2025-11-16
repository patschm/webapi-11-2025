using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ControllerWeb.Middleware;

public class Custom2
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Custom2> _logger;

    public Custom2(RequestDelegate next, ILogger<Custom2> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        _logger.LogInformation("Custom2 Incoming...");
        await _next(httpContext);
        _logger.LogInformation("Custom2 Outgoing...");
    }
}

public static class Custom2Extensions
{
    public static IApplicationBuilder UseCustom2(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Custom2>();
    }
}
