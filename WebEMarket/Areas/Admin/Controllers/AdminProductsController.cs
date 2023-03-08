using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebEMarket.Models;

namespace WebEMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly dbMarketsContext _context;

        public AdminProductsController(dbMarketsContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int CatID = 0 )
        {
            //3.2 Paged List uses 3 params: pageNumber, pageSize, IsCustomer
            var pageNumber = page; //define numbers of page
            var pageSize = 5;

            List<Product> IsProducts = new List<Product>();

            if(CatID != 0)
            {
                IsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.CatId==CatID)
                .Include(x => x.Cat) //get Category from db Product-Category table
                .OrderByDescending(x => x.ProductId).ToList();
            }
            else
            {
                IsProducts = _context.Products
                .AsNoTracking()
                .Include(x => x.Cat) //get Category from db Product-Category table
                .OrderByDescending(x => x.ProductId).ToList();
            } 
            PagedList<Product> models = new PagedList<Product>(IsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);

            return View(models);
        }

        //Perform Fillter for Category
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if(CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitslnStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
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
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitslnStock")] Product product)
        {
            if (id != product.ProductId)
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
                    if (!ProductExists(product.ProductId))
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
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
