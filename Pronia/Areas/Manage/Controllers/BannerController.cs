using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Helper.FileManager;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BannerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ProniaContext _context;

        public BannerController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Banners.AsQueryable();

            return View(PaginatedList<Banner>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
           
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Banner banner)
        {
            if (!ModelState.IsValid)
            { 
                return View();
            }
            if (banner.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please add image");
                return View();
            }

           
           banner.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/banners", banner.ImageFile);
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Banner banner = _context.Banners.FirstOrDefault(x => x.Id == id);
            if (banner == null)
            {
                return View("Error");
            }
            return View(banner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return View(banner);
            }
            Banner existBanner = _context.Banners.FirstOrDefault(x => x.Id == banner.Id);
            if (existBanner == null)
            {
                return View("Error");
            }
            existBanner.Title = banner.Title;
            existBanner.Description = banner.Description;

            existBanner.ButtonText = banner.ButtonText;
            existBanner.ButtonUrl = banner.ButtonUrl;

            string oldFileImageName = null;
            if (banner.ImageFile != null)
            {
                oldFileImageName = existBanner.ImageUrl;
                existBanner.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/banners", banner.ImageFile);
            }
            if (oldFileImageName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/banners", oldFileImageName);
            }
           
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Banner banner = _context.Banners.FirstOrDefault(x => x.Id == id);
            if (banner == null)
            {
                return BadRequest();
            }
            else
            {
                if (banner.ImageUrl != null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/banners", banner.ImageUrl);
                }
               
            }
            _context.Banners.Remove(banner);
            _context.SaveChanges();
            return Ok();

        }

    }
}
