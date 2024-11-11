using Microsoft.AspNetCore.Mvc;

namespace GOCAPUI.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
