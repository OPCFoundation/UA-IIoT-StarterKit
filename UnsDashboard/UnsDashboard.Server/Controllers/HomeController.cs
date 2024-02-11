using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnsDashboard.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        // GET: HomeController
        public ActionResult Index()
        {
            return Ok("Hello World!");
        }

        // POST: HomeController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
