using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly ProniaContext _context;

        public CategoryController(ProniaContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Categories.Include(x => x.Plants).AsQueryable();
            return View(PaginatedList<Category>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (_context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "Category Name already used");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existCategory == null)
            {
                return View("Error");
            }
            if (category.Name != existCategory.Name && _context.Tags.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "This Name already used");
                return View(category);
            }
            existCategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("index");

        }



    }
}
