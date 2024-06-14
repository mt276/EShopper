using EShop.Data;
using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;

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
        public IActionResult Index()
        {
            return View("Cart",GetCart());
        }
        public IActionResult AddToCart(int productID)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return View("Cart", GetCart());
        }
        public IActionResult UpdateCart(int productID)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, -1);
                if (cart.Lines.FirstOrDefault(l => l.Product.ProductID == productID)?.Quantity <= 0)
                {
                    cart.RemoveLine(product);
                }
                SaveCart(cart);
            }            
            return View("Cart", GetCart());
        }


        public IActionResult RemoveFromCart(int productID)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return View("Cart", GetCart());
        }
        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("cart", cart);
        }
    }
}
