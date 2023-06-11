using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Enums;
using Pronia.Helper.FileManager;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PlantController : Controller
    {
        private readonly ProniaContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Plants.Include(x => x.Category).Include(x => x.Images.Where(x => x.ImageStatus == Enums.ImageStatus.Poster)).AsQueryable();

            return View(PaginatedList<Plant>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Plant plant)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!_context.Categories.Any(x => x.Id == plant.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Please choose correctly");
                return View();
            };
            if (!Enum.IsDefined(typeof(PlantSize), plant.Size))
            {
                ModelState.AddModelError("Size", "Please choose correctly");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (plant.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage", "Posterimage is required");
                return View();
            }
            if (plant.HoverImage == null)
            {
                ModelState.AddModelError("HoverImage", "Posterimage is required");
                return View();
            }
            PlantImage posterImage = new PlantImage()
            {
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.PosterImage),
                ImageStatus = ImageStatus.Poster,
                Plant = plant,
            };
            plant.Images.Add(posterImage);
            PlantImage hoverImage = new PlantImage()
            {
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.HoverImage),
                ImageStatus = ImageStatus.Hover,
                Plant = plant,
            };
            plant.Images.Add(hoverImage);

            if (plant.AllImages.Count > 0)
            {
                foreach (var image in plant.AllImages)
                {
                    PlantImage allImages = new PlantImage()
                    {
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", image),
                        ImageStatus = ImageStatus.Images,

                    };
                    plant.Images.Add(allImages);
                }
            }

            if (plant.TagIds.Count > 0)
            {
                foreach (var tag in plant.TagIds)
                {
                    PlantTag plantTag = new PlantTag()
                    {
                        TagId = tag,

                    };
                    plant.Tags.Add(plantTag);
                }
            }

            _context.Plants.Add(plant);
            _context.SaveChanges();



            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Plant plant = _context.Plants.Include(x => x.Images).Include(x => x.Category).Include(x => x.Tags).FirstOrDefault(x => x.Id == id);
            if (plant == null)
            {
                return View("Error");
            }
            plant.TagIds = plant.Tags.Select(x => x.TagId).ToList();
            return View(plant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Plant plant)
        {
            Plant existPlant = _context.Plants.Include(x => x.Tags).Include(x => x.Category).Include(x => x.Images).FirstOrDefault(x => x.Id == plant.Id);
            if (existPlant == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (plant.CategoryId != existPlant.CategoryId && !_context.Categories.Any(x => x.Id == plant.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");
            }
            if (plant.Size != existPlant.Size && !Enum.IsDefined(typeof(PlantSize), plant.Size))
            {
                ModelState.AddModelError("Size", "Please choose correctly");
                return View();
            }
            string oldPosterImage = null;
            if (plant.PosterImage != null)
            {
                PlantImage poster = existPlant.Images.FirstOrDefault(x => x.ImageStatus == ImageStatus.Poster);
                oldPosterImage = poster?.ImageName;
                if (poster == null)
                {
                    poster = new PlantImage()
                    {
                        ImageStatus = ImageStatus.Poster,
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.PosterImage)
                    };
                    existPlant.Images.Add(poster);
                }
                else
                {
                    poster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.PosterImage);
                }
            }
            string oldHoverImage = null;
            if (plant.HoverImage != null)
            {
                PlantImage hover = existPlant.Images.FirstOrDefault(x => x.ImageStatus == ImageStatus.Hover);
                oldHoverImage = hover?.ImageName;
                if (hover == null)
                {
                    hover = new PlantImage()
                    {
                        ImageStatus = ImageStatus.Hover,
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.HoverImage)
                    };
                    existPlant.Images.Add(hover);
                }
                else
                {
                    hover.ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.HoverImage);

                }
            }
            existPlant.Tags.RemoveAll(x => !plant.TagIds.Contains(x.Id));
            var newTagIds = plant.TagIds.Where(x => !existPlant.Tags.Any(y => y.TagId == x));

            foreach (var tagId in newTagIds)
            {
                PlantTag tags = new PlantTag()
                {
                    TagId = tagId,

                };
                existPlant.Tags.Add(tags);
            }
            var removedImages = existPlant.Images.FindAll(x => x.ImageStatus == ImageStatus.Images && !plant.ImageIds.Contains(x.Id));
            existPlant.Images.RemoveAll(x => x.ImageStatus == ImageStatus.Images && !plant.ImageIds.Contains(x.Id));
            foreach (var item in plant.AllImages)
            {
                PlantImage image = new PlantImage()
                {
                    ImageStatus = ImageStatus.Images,
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", item)
                };
                existPlant.Images.Add(image);
            }
            existPlant.Name = plant.Name;
            existPlant.Description = plant.Description;
            existPlant.CostPrice = plant.CostPrice;
            existPlant.SalePrice = plant.SalePrice;
            existPlant.DiscountPercent = plant.DiscountPercent;
            existPlant.Rate = plant.Rate;
            existPlant.CategoryId = plant.CategoryId;
            existPlant.isFeatured = plant.isFeatured;
            existPlant.isNew = plant.isNew;
            existPlant.StockStatus = plant.StockStatus;
            existPlant.Size = plant.Size;

            _context.SaveChanges();
            if (oldPosterImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/plants", oldPosterImage);
            }
            if (oldHoverImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/plants", oldHoverImage);
            }
            if (removedImages.Any())
            {
                FileManager.DeleteAll(_env.WebRootPath, "uploads/plants", removedImages.Select(x => x.ImageName).ToList());
            }
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Plant plant = _context.Plants.Include(x=>x.Images).FirstOrDefault(x => x.Id == id);
            if(plant == null )
            {
                return BadRequest();

            }
            else
            {
                if (plant.Images.Any(x => x.ImageStatus == ImageStatus.Poster))
                {
                    FileManager.Delete(_env.WebRootPath,"uploads/plants",plant.Images.FirstOrDefault(x=>x.ImageStatus==ImageStatus.Poster).ImageName);
                }
                if (plant.Images.Any(x => x.ImageStatus == ImageStatus.Hover))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/plants", plant.Images.FirstOrDefault(x => x.ImageStatus == ImageStatus.Hover).ImageName);
                }
                if (plant.Images.Any(x => x.ImageStatus == ImageStatus.Images))
                {
                    FileManager.DeleteAll(_env.WebRootPath, "uploads/plants", plant.Images.Where(x => x.ImageStatus == ImageStatus.Poster).Select(x=>x.ImageName).ToList());
                }
                _context.Plants.Remove(plant);
                _context.SaveChanges();

                return Ok();

            }
         
        }
    }
}
