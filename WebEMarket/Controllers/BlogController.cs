using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Linq;
using System.Threading.Tasks;
using WebEMarket.Models;

namespace WebEMarket.Controllers
{
    public class BlogController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notyfService { get; }

        public BlogController(dbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        //GET: Blogs/Index
        [Route("blogs.html", Name = "Blog")]
        public async Task<IActionResult> Index(int? page)
        {
            //3.2 Paged List uses 3 params: pageNumber, pageSize, IsCustomer
            var pageNumber = page == null || page <= 0 ? 1 : page.Value; //define numbers of page
            var pageSize = 10;
            var IsTinTucs = _context.TinTucs
                .AsNoTracking()
                .OrderByDescending(x => x.PostId);

            PagedList<TinTuc> models = new PagedList<TinTuc>(IsTinTucs, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        //GET: Blogs/Details/id
        [Route("/tin-tuc/{Alias}-{id}.html", Name = "Details")]
        public IActionResult Details(int id)
        {
            var tinTuc = _context.TinTucs
                .AsNoTracking()
                .SingleOrDefault(x => x.PostId == id);

            if (tinTuc == null)
            {
                return RedirectToAction("Index");
            }
            var lsBaivietlienquan = _context.TinTucs
                .AsNoTracking()
                .Where(x => x.Published == true && x.PostId != id)
                .Take(3)
                .OrderByDescending(x =>x.CreatedDate)
                .ToList();
            ViewBag.Baivietlienquan = lsBaivietlienquan;
            return View(tinTuc);
        }
    }
}
