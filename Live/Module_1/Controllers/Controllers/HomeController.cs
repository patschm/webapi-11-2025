using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("home")]
public class HomeController: ControllerBase
{
    // Action
    //[Route("/")]
    [HttpGet("/index")]
    public string Index()
    {
        return "Hello from Controller";
    }
}