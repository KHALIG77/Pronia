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
    public class SliderController : Controller
    {


        private readonly IWebHostEnvironment _env;
        private readonly ProniaContext _context;

        public SliderController(ProniaContext context,IWebHostEnvironment env) 
        {
          _context=context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            var query=_context.Sliders.AsQueryable();

            return View(PaginatedList<Slider>.Create(query,page,2));
        }
        public IActionResult Create()
        {
            var slide = new Slider();
            slide.Order=_context.Sliders.Any()?_context.Sliders.Max(x=>x.Order)+1:1;
            return View(slide);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
               
                return View();
            }
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please add image");
                return View();
            }
            
            if (slider.Order>(_context.Sliders.Any()?_context.Sliders.Max(x=>x.Order)+1:1))
            {

                ModelState.AddModelError("Order", "Please write what is offered:" + $"{_context.Sliders.Max(x => x.Order) + 1}" );
                slider.Order = _context.Sliders.Max(x => x.Order) + 1;
              
                return View(slider);

            }
            foreach (var item in _context.Sliders.Where(x => x.Order >= slider.Order))
            {
                item.Order++;
            }
            if(slider.BgImageFile!=null)
            {
                slider.BgUrl = FileManager.Save(_env.WebRootPath, "uploads/slider-bg", slider.BgImageFile);
            }
            
            slider.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/slider-inner", slider.ImageFile);
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("index");
    }
        public IActionResult Edit(int id )
        {
            Slider slider=_context.Sliders.FirstOrDefault(x=>x.Id==id); 
            if(slider==null)
            {
                return View("Error");
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }
            Slider existSlider=_context.Sliders.FirstOrDefault(x=>x.Id==slider.Id);
            if (existSlider == null)
            {
                return View("Error");
            }
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.Order=slider.Order;
            existSlider.ButtonText=slider.ButtonText;
            existSlider.ButtonUrl=slider.ButtonUrl;

            string oldFileImageName = null;
            if(slider.ImageFile!=null)
            {
                oldFileImageName = existSlider.ImageUrl;
                existSlider.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/slider-inner", slider.ImageFile);
            }
            if(oldFileImageName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/slider-inner", oldFileImageName);
            }
            string oldFileBgName = null;
            if(slider.BgImageFile!=null)
            {
                oldFileBgName=existSlider.BgUrl;
                existSlider.BgUrl = FileManager.Save(_env.WebRootPath, "uploads/slider-bg", slider.BgImageFile);

            }
            if(oldFileBgName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/slider-bg", oldFileBgName);
            }
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Slider slider =_context.Sliders.FirstOrDefault(x=>x.Id== id);   
            if(slider == null)
            {
                return BadRequest();
            }
            else
            {
               if(slider.ImageUrl!=null && slider.BgUrl==null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/slider-inner", slider.ImageUrl);       
                }
               else if(slider.ImageUrl != null && slider.BgUrl != null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/slider-inner", slider.ImageUrl);
                    FileManager.Delete(_env.WebRootPath, "uploads/slider-bg", slider.BgUrl);

                }
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges(); 
            return Ok();

        }
}}
