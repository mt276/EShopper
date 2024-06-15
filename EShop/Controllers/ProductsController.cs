using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Data;
using EShop.Models;
using EShop.Models.ViewModels;
using static EShop.Controllers.ProductsController;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        int pageSize = 9;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class PriceRange
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }
        [HttpPost]

        
        public IActionResult GetFilteredProducts([FromBody] FilterData filterData)
        {
            var filterProducts = _context.Products.ToList();
            if (filterData.PriceRanges != null && filterData.PriceRanges.Count > 0 && !filterData.PriceRanges.Contains("all"))
            {
                List<PriceRange> priceRanges = new List<PriceRange>();
                foreach (var range in filterData.PriceRanges)
                {
                    var value = range.Split("-").ToArray();
                    PriceRange priceRange = new PriceRange();
                    priceRange.Min = Int16.Parse(value[0]);
                    priceRange.Max = Int16.Parse(value[1]);
                    priceRanges.Add(priceRange);
                }
                filterProducts = filterProducts.Where(p=>priceRanges.Any(r=>p.ProductPrice >= r.Min && p.ProductPrice <= r.Max)).ToList();
            }
            if (filterData.Colors != null && filterData.Colors.Count > 0 && !filterData.Colors.Contains("all"))
            {
                filterProducts = filterProducts.Where(p => filterData.Colors.Contains(p.Color?.ColorName)).ToList();

            }
            if (filterData.Sizes != null && filterData.Sizes.Count > 0 && !filterData.Sizes.Contains("all"))
            {
                filterProducts = filterProducts.Where(p => filterData.Sizes.Contains(p.Size?.SizeName)).ToList();

            }
            return PartialView("_ReturnProducts", filterProducts);
        }

        // GET: Products

        public Task<IActionResult> Index(int productPage = 1)
        {

            return Task.FromResult<IActionResult>(View(
                new ProductListViewModel
                {
                    Products = _context.Products
                    .Skip((productPage - 1) * pageSize)
                    .Take(pageSize),
                    PagingInfo = new PagingInfo()
                    {
                        ItemsPerPage = pageSize,
                        CurrentPage = productPage,
                        TotalItems = _context.Products.Count()

                    }
                }));

        }
        [HttpPost]
        public IActionResult SearchProduct(string keywords, int productPage = 1)
        {

            return View("Index",
                new ProductListViewModel
                {
                    Products = _context.Products
                    .Where(p => p.ProductName.Contains(keywords))
                    .Skip((productPage - 1) * pageSize)
                    .Take(pageSize),
                    PagingInfo = new PagingInfo()
                    {
                        ItemsPerPage = pageSize,
                        CurrentPage = productPage,
                        TotalItems = _context.Products.Count()

                    }
                }
            );

        }
        public async Task<IActionResult> ProductsByCat(int categoryID)
        {
            var applicationDbContext = _context.Products.Where(p => p.CategoryID == categoryID);
            return View("Index", await applicationDbContext.ToListAsync());
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID");
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID");
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductDescription,CategoryID,ProductPrice,ProductDiscount,ProductPhoto,SizeID,ColorID,IsTrandy,IsArrived")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", product.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", product.SizeID);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", product.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", product.SizeID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductDescription,CategoryID,ProductPrice,ProductDiscount,ProductPhoto,SizeID,ColorID,IsTrandy,IsArrived")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "ColorID", product.ColorID);
            ViewData["SizeID"] = new SelectList(_context.Sizes, "SizeID", "SizeID", product.SizeID);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
