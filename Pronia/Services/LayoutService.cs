using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Pronia.DAL;
using Pronia.ViewModels;

namespace Pronia.Services
{
	public class LayoutService
	{
		private readonly ProniaContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LayoutService(ProniaContext context,IHttpContextAccessor httpContextAccessor)
        {
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}
		public Dictionary<string,string> GetSettings() 
		{
			return _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);   
		}
        public BasketViewModel GetBasket()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && _httpContextAccessor.HttpContext.User.IsInRole("Member"))
            {
                var basketItems = _context.BasketItems.Include(x => x.Plant).ThenInclude(x => x.Images.Where(x=>x.ImageStatus==Enums.ImageStatus.Poster)).Where(x => x.AppUserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
                var bv = new BasketViewModel();

                foreach (var ci in basketItems)
                {
                    BasketItemViewModel bi = new BasketItemViewModel
                    {
                        Count = (int)ci.Count,
                        Plant = ci.Plant,

                    };
                    bv.BasketItems.Add(bi);
                    bv.TotalPrice += (bi.Plant.DiscountPercent > 0 ? ((bi.Plant.SalePrice * (100 - bi.Plant.DiscountPercent) ) / 100) : bi.Plant.SalePrice * bi.Count);
                }
                return bv;

            }
            else
            {
                var bv = new BasketViewModel();
                var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
                if (basketJson != null)
                {
                    var cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketJson);

                    foreach (var ci in cookieItems)
                    {
                        BasketItemViewModel bi = new BasketItemViewModel
                        {
                            Count = (int)ci.Count,
                            Plant = _context.Plants.Include(x => x.Images.Where(x=>x.ImageStatus==Enums.ImageStatus.Poster)).FirstOrDefault(x => x.Id == ci.PlantId),

                        };
                        bv.BasketItems.Add(bi);
                        bv.TotalPrice += (bi.Plant.DiscountPercent > 0 ? (bi.Plant.SalePrice * (100 - bi.Plant.DiscountPercent) / 100) : bi.Plant.SalePrice) * bi.Count;
                    }
                }

                return bv;
            }

        }
      
    }
}
