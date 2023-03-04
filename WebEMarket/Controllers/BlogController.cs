using Microsoft.AspNetCore.Mvc;

namespace WebEMarket.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
