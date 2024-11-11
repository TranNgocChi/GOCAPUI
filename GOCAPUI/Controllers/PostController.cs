using Microsoft.AspNetCore.Mvc;

namespace GOCAPUI.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
