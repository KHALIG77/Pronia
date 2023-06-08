using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            Plant plant = _context.Plants.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            if (plant == null)
            {
                return View("Error");
            }
            QuickModalViewModel quickVM = new QuickModalViewModel()
            {
                Plant = plant,
                Features = _context.Features.Take(3).ToList()
            };
            return PartialView("_PlantDetailModalPartialView", quickVM);
        }

        public IActionResult AddToBasket(int id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItem = _context.BasketItems.FirstOrDefault(x => x.PlantId == id && x.AppUserId == userId);

                if (basketItem != null)
                {
                    basketItem.Count++;
                }
                else
                {
                    basketItem = new BasketItem() { AppUserId = userId, PlantId = id, Count = 1 };
                    _context.BasketItems.Add(basketItem);


                }
                _context.SaveChanges();
                var basketItems = _context.BasketItems.Include(x => x.Plant).ThenInclude(x => x.Images.Where(x=>x.ImageStatus==Enums.ImageStatus.Poster)).Where(x => x.AppUserId == userId).ToList();


                return PartialView("BasketItemPartialView", GenerateBasketVM(basketItems));

            }
            else
            {
                List<BasketItemCookieViewModel> cookieItems = new List<BasketItemCookieViewModel>();
                BasketItemCookieViewModel cookieitem;
                var basketStr = Request.Cookies["Basket"];

                if (basketStr != null)
                {
                    cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
                    cookieitem = cookieItems.FirstOrDefault(x => x.PlantId == id);
                    if (cookieitem != null)
                    {
                        cookieitem.Count++;
                    }
                    else
                    {
                        cookieitem = new BasketItemCookieViewModel { PlantId = id, Count = 1 };
                        cookieItems.Add(cookieitem);
                        HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));

                    
                    }
                }
                else
                {
                    cookieitem = new BasketItemCookieViewModel { PlantId = id, Count = 1 };
                    cookieItems.Add(cookieitem);
                    

                }


                HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));



                return PartialView("BasketItemPartialView", GenerateBasketVM(cookieItems));
         
            }


        }
        public IActionResult RemoveBasket(int id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItem = _context.BasketItems.Include(x => x.Plant).ThenInclude(x => x.Images.Where(x=>x.ImageStatus==Enums.ImageStatus.Poster)).FirstOrDefault(x => x.PlantId == id && x.AppUserId == userId);
                if (basketItem != null)
                {
                    if (basketItem.Count == 1)
                    {
                        _context.BasketItems.Remove(basketItem);
                    }
                    else
                    {
                        basketItem.Count--;
                    }
                }
                _context.SaveChanges();
                var basketItems = _context.BasketItems.Include(x => x.Plant).ThenInclude(x => x.Images.Where(x=>x.ImageStatus==Enums.ImageStatus.Poster)).Where(x => x.AppUserId == userId).ToList();

                return PartialView("BasketItemPartialView", GenerateBasketVM(basketItems));


            }
            else
            {
                List<BasketItemCookieViewModel> cookieItems = new List<BasketItemCookieViewModel>();
                var basketStr = HttpContext.Request.Cookies["basket"];
                if (basketStr != null)
                {
                    cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
                    var item = cookieItems.FirstOrDefault(x => x.PlantId == id);
                    BasketViewModel bv = new BasketViewModel();
                    if (item != null)
                    {
                        if (item.Count == 1)
                        {
                            cookieItems.Remove(item);
                        }
                        else
                        {
                            item.Count--;
                        }

                        Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));

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
                    return PartialView("BasketItemPartialView", bv);


                }
                else
                {
                    return NotFound();
                }
            }




        }


        private BasketViewModel GenerateBasketVM(List<BasketItemCookieViewModel> cookieItems)
        {
            BasketViewModel bv = new BasketViewModel();
            foreach (var ci in cookieItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = (int)ci.Count,
                    Plant = _context.Plants.Include(x=>x.Images.Where(x=>x.ImageStatus==Enums.ImageStatus.Poster)).FirstOrDefault(x=>x.Id==ci.PlantId),

                };
                bv.BasketItems.Add(bi);
                bv.AllCount =(byte)cookieItems.Count;
                bv.TotalPrice += (bi.Plant.DiscountPercent > 0 ? ((bi.Plant.SalePrice * (100 - bi.Plant.DiscountPercent) / 100)*bi.Count) : bi.Plant.SalePrice * bi.Count);
            }
            return bv;

        }
        private BasketViewModel GenerateBasketVM(List<BasketItem> basketItems)
        {
            BasketViewModel bv = new BasketViewModel();
            foreach (var item in basketItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = (int)item.Count,
                    Plant = item.Plant,
                    

                };
                bv.AllCount=(byte)basketItems.Count;
                bv.BasketItems.Add(bi);
                bv.TotalPrice += (bi.Plant.DiscountPercent > 0 ? ((bi.Plant.SalePrice * (100 - bi.Plant.DiscountPercent) ) / 100) : bi.Plant.SalePrice * bi.Count);
            }
            return bv;
        }

    }
}
