using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View("Contact");
        }
    }
}
