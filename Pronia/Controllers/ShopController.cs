using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Enums;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProniaContext _context;

        public ShopController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId,int? minprice, int? maxprice, List<int> tagId=null,string size=null,string sort=null)
        {
            ShopViewModel shopVM = new ShopViewModel()
            {
                Categories = _context.Categories.Include(x => x.Plants).ToList(),
                Tags = _context.Tags.ToList(),
                PlantSize = Enum.GetNames(typeof(PlantSize)).ToList(),
                AllPlantCount = _context.Plants.Count()

            };
            var query = _context.Plants.Include(x => x.Images.Where(x => x.ImageStatus != ImageStatus.Images)).Include(x => x.Category).Include(x => x.Tags).AsQueryable();
            if (sort!=null)
            {
                switch (sort)
                {
                    case "AToZ":
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "ZToA":
                        query = query.OrderByDescending(x=>x.Name);
                        break;
                    case "LowToHigh":
                        query = query.OrderBy(x => x.SalePrice);
                            break;
                    case "HighToLow":
                        query = query.OrderByDescending(x => x.SalePrice);
                        break;
                }
            }
            if(categoryId!= null)
            {
                query=query.Where(x=>x.CategoryId==categoryId);
            }
            if (size!=null)
            {
                query=query.Where(x=>x.Size.ToString()==size.ToString());
            }
           
           
           ViewBag.SortList = new List<SelectListItem>
            {
                new SelectListItem{ Value="AToZ",Text="Sort By Name (A-Z)",Selected=sort=="AToZ"},
                new SelectListItem{ Value="ZToA",Text="Sort By Name (A-Z)",Selected=sort=="ZToA"},
                new SelectListItem{ Value="LowToHigh",Text="Sort By Price(LOW-HIGH)",Selected=sort=="LowToHigh"},
                new SelectListItem{ Value="HighToLow",Text="Sort By Price (HIGH-LOW)",Selected=sort=="HighToLow"}


            };

            ViewBag.MinRangeLimit = _context.Plants.Any() ? _context.Plants.Min(x => x.SalePrice) : 0;
            ViewBag.MaxRangeLimit = _context.Plants.Any() ? _context.Plants.Max(X => X.SalePrice) : 0;
            ViewBag.MinPrice = minprice;
            ViewBag.MaxPrice = maxprice;
           
            shopVM.Plants = query.ToList();


            return View(shopVM);
        }

    }
}
