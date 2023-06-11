using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Enums;
using Pronia.Models;
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
        public IActionResult Index(int? categoryid,int? minprice, int? maxprice,string search=null, List<int> tagId = null, string  size=null,string sort=null,int page=1)
        {
            ShopViewModel shopVM = new ShopViewModel()
            {
                Categories = _context.Categories.Include(x => x.Plants).ToList(),
                Tags = _context.Tags.ToList(),
                PlantSize = Enum.GetNames(typeof(PlantSize)).ToList(),
                AllPlantCount = _context.Plants.Count()

            };
            var query = _context.Plants.Include(x => x.Images.Where(x => x.ImageStatus != ImageStatus.Images)).Include(x => x.Category).Include(x => x.Tags).ThenInclude(y=>y.Tag).AsQueryable();
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
            if(categoryid!= null)
            {
                query=query.Where(x=>x.CategoryId==categoryid);
            }
            if (size!=null)
            {
                query=query.Where(x=>(int)x.Size==(int)Enum.Parse(typeof(PlantSize),size));
            }
            //if (_context.Plants.Any()&&minprice==null)
            //{
            //    minprice =(int)_context.Plants.Min(x => x.SalePrice);
            //}else if (_context.Plants.Any()&&maxprice!=null)
            //{
            //    maxprice =(int) _context.Plants.Max(X => X.SalePrice);
            if (minprice!=null&&maxprice!=null)
            {
                query = query.Where(x=>x.SalePrice>=minprice&&x.SalePrice<=maxprice);
            }
            if (tagId.Count > 0)
            {


                query = query.Include(x => x.Tags.Where(x=>x.TagId==20));


            }

            if (search!=null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
           
           ViewBag.SortList = new List<SelectListItem>
            {
                new SelectListItem{ Value="AToZ",Text="Sort By Name (A-Z)",Selected=sort=="AToZ"},
                new SelectListItem{ Value="ZToA",Text="Sort By Name (Z-A)",Selected=sort=="ZToA"},
                new SelectListItem{ Value="LowToHigh",Text="Sort By Price(LOW-HIGH)",Selected=sort=="LowToHigh"},
                new SelectListItem{ Value="HighToLow",Text="Sort By Price (HIGH-LOW)",Selected=sort=="HighToLow"}


            };
            ViewBag.CategoryId=categoryid;
            ViewBag.Size=size;
            ViewBag.MinRangeLimit = _context.Plants.Any() ? _context.Plants.Min(x => x.SalePrice) : 0;
            ViewBag.MaxRangeLimit = _context.Plants.Any() ? _context.Plants.Max(X => X.SalePrice) : 0;
            ViewBag.MinPrice = minprice;
            ViewBag.MaxPrice = maxprice;
            ViewBag.TagIds = tagId;
            ViewBag.Search=search;

            query.ToList();

            shopVM.PaginatedList= PaginatedList<Plant>.Create(query, page, 6);
          


            return View(shopVM);
        }

    }
}
