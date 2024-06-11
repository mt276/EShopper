using EShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class Navbar:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Navbar(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Default",_context.Categories.ToList());
        }
    }
}
