using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChatServer.Models;
using Microsoft.AspNetCore.SignalR;
using ChatServer.Hubs;

namespace ChatServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<IRCIIHub> _hub;

    public HomeController(ILogger<HomeController> logger, IHubContext<IRCIIHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    public IActionResult Index()
    {
        //_hub.Clients.All.SendAsync("Hoi");
        return View("Ircii");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
