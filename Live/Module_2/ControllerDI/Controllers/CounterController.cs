using Microsoft.AspNetCore.Mvc;
using Services;

namespace ControllerDI.Controllers;

[ApiController]
[Route("[controller]")]
public class CounterController : ControllerBase
{
    private readonly MisCounter _counter;

    public CounterController(MisCounter counter)
    {
        _counter = counter;
    }

    [HttpGet]
    public int Get()
    {
        //var counter = new Counter();
        _counter.Increment();
        return _counter.Value;
    }

}
