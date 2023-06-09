using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Helper.FileManager;
using Pronia.Migrations;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BrandController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ProniaContext _context;

        public BrandController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Brands.AsQueryable();

            return View(PaginatedList<Brand>.Create(query, page, 2));
        }
        public IActionResult Create()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (brand.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please add image");
                return View();
            }



            brand.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/brands", brand.ImageFile);
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
      
        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);
            if (brand == null)
            {
                return BadRequest();
            }
            else
            {
                if (brand.ImageUrl != null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/brands", brand.ImageUrl);
                }

            }
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return Ok();

        }
    
}
}
