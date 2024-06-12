using EShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class JustArrived : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public JustArrived(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Default", _context.Products.Where(p=>p.IsArrived==true).ToList());
        }
    }
}
