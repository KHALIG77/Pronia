using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
	public class HomeController : Controller
	{
        private readonly ProniaContext _context;

        public HomeController(ProniaContext context)
        {
           _context = context;
        }
		
        public IActionResult Index()
		{
			HomeViewModel homeVM = new HomeViewModel()
			{
				Slider = _context.Sliders.OrderBy(x=>x.Order).ToList(),
				Feature=_context.Features.ToList(),
				FeaturedPlant=_context.Plants.Include(x=>x.Images).Include(x=>x.Category).Include(x=>x.Tags).Where(x=>x.isFeatured==true).Take(8).ToList(),
                IsNewPlant = _context.Plants.Include(x => x.Images).Include(x => x.Category).Include(x => x.Tags).Where(x => x.isNew == true).Take(8).ToList(),
                DiscountedPlant = _context.Plants.Include(x => x.Images).Include(x => x.Category).Include(x => x.Tags).Where(x => x.DiscountPercent>0).Take(8).ToList(),
				Rated =  _context.Plants.Include(x => x.Images).Include(x => x.Category).Include(x => x.Tags).Where(x => x.Rate>3).Take(4).ToList(),
				Banner=_context.Banners.ToList(),
				Brand=_context.Brands.ToList(),
				Comments=_context.PlantComments.Include(user=>user.AppUser).Where(comment=>comment.ShowComment==true).Take(3).ToList(),

            };
			
			return View(homeVM);	
		}
	
	}
}