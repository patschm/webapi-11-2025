using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControllerWeb.Filters;

public class CustomFilterAttribute : ActionFilterAttribute
{
    private readonly ILogger<CustomFilterAttribute> _logger;

    public CustomFilterAttribute(ILogger<CustomFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("*** Action Executing");
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("*** Action Executed");
    }
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        _logger.LogInformation("*** Result Executing");
    }
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogInformation("*** Result Executed");
    }
}
