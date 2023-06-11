using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class TagController : Controller
    {
        private readonly ProniaContext _context;

        public TagController(ProniaContext context)
        {
            
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            var query =_context.Tags.Include(x=>x.Plants).AsQueryable();
            return View(PaginatedList<Tag>.Create(query, page, 2));
           
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            if (_context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "Tag Name already used");
                return View();
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Tag tag=_context.Tags.FirstOrDefault(x=>x.Id==id);
            if(tag!=null)
            {
                _context.Tags.Remove(tag);
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
            Tag tag=_context.Tags.FirstOrDefault(y=>y.Id==id);  
            if( tag!=null )
            {
                return View(tag);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag tag)
        {
           Tag existTag =_context.Tags.FirstOrDefault(x=>x.Id== tag.Id); 
           if(existTag==null )
            {
                return View("Error");
            }
           if(tag.Name!=existTag.Name && _context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "This Name already used");
                return View(tag);
            }
           existTag.Name=tag.Name;
            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
