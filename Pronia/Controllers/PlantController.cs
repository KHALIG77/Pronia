using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class PlantController : Controller
    {
        private readonly ProniaContext _context;

        public PlantController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult ModalDetail(int id)
        {
            Plant plant=_context.Plants.Include(x=>x.Category).Include(x=>x.Tags).Include(x=>x.Images).FirstOrDefault(x=>x.Id==id);
            if(plant==null)
            {
                return View("Error");
            }
            QuickModalViewModel quickVM= new QuickModalViewModel()
            {
                Plant=plant,
                Features=_context.Features.Take(3).ToList()
            };
            return PartialView("_PlantDetailModalPartialView",quickVM);
        }
    }
}
