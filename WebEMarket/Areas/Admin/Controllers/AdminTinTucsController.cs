using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebEMarket.Helper;
using WebEMarket.Models;

namespace WebEMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTinTucsController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notyfService { get; }

        public AdminTinTucsController(dbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminTinTucs
        public IActionResult Index(int? page)
        {
            var collection = _context.TinTucs.AsNoTracking().ToList();
            foreach (var item in collection)
            {
                if (item.CreatedDate == null)
                {
                    item.CreatedDate = DateTime.Now;
                    _context.Update(item);
                    _context.SaveChangesAsync();
                }
            }

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

        // GET: Admin/AdminTinTucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // GET: Admin/AdminTinTucs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTinTucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Scontents,Contents,Thumb,Published,Alias,CreatedDate,Author,AccountId,Tags,CatId,IsHot,IsNewfeed,MetaKey,MetaDesc,Views")] TinTuc tinTuc, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(tinTuc.Title) + extension;
                    tinTuc.Thumb = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                }

                if (string.IsNullOrEmpty(tinTuc.Thumb)) tinTuc.Thumb = "default.jpg";
                tinTuc.Alias = Utilities.SEOUrl(tinTuc.Title);
                tinTuc.CreatedDate = DateTime.Now;
                _context.Add(tinTuc);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm tin thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(tinTuc);
        }

        // GET: Admin/AdminTinTucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }
            return View(tinTuc);
        }

        // POST: Admin/AdminTinTucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Scontents,Contents,Thumb,Published,Alias,CreatedDate,Author,AccountId,Tags,CatId,IsHot,IsNewfeed,MetaKey,MetaDesc,Views")] TinTuc tinTuc, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != tinTuc.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(tinTuc.Title) + extension;
                        tinTuc.Thumb = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                    }

                    if (string.IsNullOrEmpty(tinTuc.Thumb)) tinTuc.Thumb = "default.jpg";
                    tinTuc.Alias = Utilities.SEOUrl(tinTuc.Title);

                    _context.Update(tinTuc);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa tin thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TinTucExists(tinTuc.PostId))
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
            return View(tinTuc);
        }

        // GET: Admin/AdminTinTucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // POST: Admin/AdminTinTucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tinTuc = await _context.TinTucs.FindAsync(id);
            _context.TinTucs.Remove(tinTuc);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xoá tin thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TinTucExists(int id)
        {
            return _context.TinTucs.Any(e => e.PostId == id);
        }
    }
}
