using EShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Components
{
    public class Trandy:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Trandy(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Default", _context.Products.Where(p=>p.IsTrandy==true).ToList());
        }
    }
}
