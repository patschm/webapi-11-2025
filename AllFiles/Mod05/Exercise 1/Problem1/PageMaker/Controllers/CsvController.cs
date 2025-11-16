using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PageMaker.Services;

namespace PageMaker.Controllers
{
    public class CsvController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 100000000)]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var stream = file.OpenReadStream();
            return View(stream);
        }
    }
}
