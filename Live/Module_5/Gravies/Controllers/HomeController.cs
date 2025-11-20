using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gravies.Models;

namespace Gravies.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogDebug("Debug Info");
        _logger.LogTrace("Trace info");
        _logger.LogInformation($"Information info {nameof(Index)} ");

        _logger.LogWarning("Warning Info");
        _logger.LogError("Error Info");
        _logger.LogCritical("Critical info");
        return View();
    }

    public IActionResult Privacy()
    {
        try
        {
            throw new Exception("ooops");
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
