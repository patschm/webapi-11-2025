using Microsoft.AspNetCore.Mvc;

namespace DeepThought.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public async Task<int> Get()
        {
            await Task.Delay(2000);
            return 42;
        }
    }
}
