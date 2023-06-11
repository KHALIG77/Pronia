using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Helper.FileManager;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class FeatureController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ProniaContext _context;

        public FeatureController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Features.AsQueryable();

            return View(PaginatedList<Feature>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                
                return View();
            }
            if (feature.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please add image");
                return View();
            }

              
           
            feature.IconUrl = FileManager.Save(_env.WebRootPath, "uploads/features", feature.ImageFile);
            _context.Features.Add(feature);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null)
            {
                return View("Error");
            }
            return View(feature);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return View(feature);
            }
            Feature existFeature = _context.Features.FirstOrDefault(x => x.Id == feature.Id);
            if (existFeature == null)
            {
                return View("Error");
            }
            existFeature.Title = feature.Title;
            existFeature.Description = feature.Description;
           
          

            string oldFileImageName = null;
            if (feature.ImageFile != null)
            {
                oldFileImageName = existFeature.IconUrl;
                existFeature.IconUrl = FileManager.Save(_env.WebRootPath, "uploads/features", feature.ImageFile);
            }
            if (oldFileImageName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/features", oldFileImageName);
            }
           
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null)
            {
                return BadRequest();
            }
            else
            {
                if (feature.IconUrl != null )
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/features", feature.IconUrl);
                }
             
            }
            _context.Features.Remove(feature);
            _context.SaveChanges();
            return Ok();

        }
    }
}
