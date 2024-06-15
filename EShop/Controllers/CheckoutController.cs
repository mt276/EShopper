using EShop.Data;
using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Cart cart = GetCart();
            return View(cart);
        }

        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }
        public IActionResult Complete()
        {
            Cart cart = GetCart();
            // Here you would normally process the payment and clear the cart
            HttpContext.Session.Remove("cart");
            return View();
        }
    }
}
