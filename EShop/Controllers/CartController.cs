using EShop.Data;
using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AddToCart(int productID)
        {
            Product? product = _context.Products.FirstOrDefault(p=>p.ProductID == productID);
            if (product == null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("Cart",Cart);
            }
            return View("Cart", Cart);
        }
    }
}
