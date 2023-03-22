using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Linq;
using WebEMarket.Models;

namespace WebEMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly dbMarketsContext _context;

        public ProductController(dbMarketsContext context)
        {
            _context = context;
        }

        [Route("shop.html", Name = "ShopProduct")]
        public IActionResult Index(int? page)
        {
            try
            {
                //3.2 Paged List uses 3 params: pageNumber, pageSize, IsCustomer
                var pageNumber = page == null || page <= 0 ? 1 : page.Value; //define numbers of page
                var pageSize = 10;
                var IsTinTucs = _context.Products
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreated);

                PagedList<Product> models = new PagedList<Product>(IsTinTucs, pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }  
        }

        [Route("danhmuc/{Alias}", Name = ("ListProduct"))]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                //3.2 Paged List uses 3 params: pageNumber, pageSize, IsCustomer
                var pageSize = 10;
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
                var IsTinTucs = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CatId == danhmuc.CatId)
                    .OrderByDescending(x => x.DateCreated);

                PagedList<Product> models = new PagedList<Product>(IsTinTucs, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                //xem product thuoc Category nao va tra ve phan tu dau tien duoc tim thay
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                var lsProduct = _context.Products.AsNoTracking().Where(x => x.CatId == product.CatId && x.ProductId != id && x.Active == true).OrderByDescending(x => x.DateCreated).Take(4).ToList();

                ViewBag.SanPham = lsProduct;


                return View(product);
            } catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
