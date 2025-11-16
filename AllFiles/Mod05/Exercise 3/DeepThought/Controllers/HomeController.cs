using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeepThinker.Interfaces;

namespace DeepThought.Controllers
{
    public class HomeController : Controller
    {
        private IThinkService _svc;

        public HomeController(IThinkService svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> Index()
        {
            var result =_svc.DeepThinkingAsync("values");
            await Task.Delay(500);
            ViewBag.Answer = result.Result;
           
            return View();
        }
    }
}
