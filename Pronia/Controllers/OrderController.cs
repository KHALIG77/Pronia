using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProniaContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(ProniaContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult>Checkout()
        {
            OrderViewModel orderVM = new OrderViewModel();
            orderVM.Items = GetCheckoutItems();
            if (User.Identity.IsAuthenticated && User.IsInRole("Member")) 
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                orderVM.OrderFormVM = new OrderFormViewModel
                {
                    Address = user.Address,
                    Email = user.Email,
                    FullName = user.FullName,

                };
                 orderVM.TotalPrice = orderVM.Items.Any() ? orderVM.Items.Sum(x => x.Price * x.Count) : 0;
                return View(orderVM);


            }
            orderVM.TotalPrice = orderVM.Items.Any() ? orderVM.Items.Sum(x => x.Price * x.Count) : 0;

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderFormViewModel orderVM)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Member"))
            {
                if (string.IsNullOrEmpty(orderVM.FullName))
                {
                    ModelState.AddModelError("FullName", "FullName is required");
                    OrderViewModel vm = new OrderViewModel();
                    vm.Items = GetCheckoutItems();
                    vm.OrderFormVM = orderVM;
                    vm.TotalPrice = vm.TotalPrice = vm.Items.Any() ? vm.Items.Sum(x => x.Price * x.Count) : 0;
                    return View("Checkout", vm);
                }
                if (string.IsNullOrEmpty(orderVM.Email))
                {
                    ModelState.AddModelError("Email", "Email is required");
                    OrderViewModel vm = new OrderViewModel();
                    vm.Items = GetCheckoutItems();
                    vm.OrderFormVM = orderVM;
                  vm.TotalPrice = vm.Items.Any() ? vm.Items.Sum(x => x.Price * x.Count) : 0;

                    return View("Checkout", vm);
                }
            }
            if (!ModelState.IsValid)
            {
                OrderViewModel vm = new OrderViewModel();
                vm.Items = GetCheckoutItems();
                vm.OrderFormVM = orderVM;
                return View("Checkout", vm);
            }


            Order order = new Order()
            {
                Address = orderVM.Address,
                Text = orderVM.Text,
                OrderStatus = Enums.OrderStatus.Pending,
                CreateAt = DateTime.UtcNow.AddHours(4),
            };
            var items = GetCheckoutItems();
            foreach (var item in items)
            {
                Plant plant = _context.Plants.Find(item.PlantId);
                OrderItem orderItem = new OrderItem
                {
                    PlantId = item.PlantId,
                    DiscountPercent = plant.DiscountPercent,
                    UnitCostPrice = plant.CostPrice,
                    UnitPrice = plant.DiscountPercent > 0 ? ((plant.SalePrice * (100 - plant.DiscountPercent) ) / 100) :plant.SalePrice,
                    Count = item.Count,

                };
                order.OrderItems.Add(orderItem);
            }
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                order.FullName = user.FullName;
                order.Email = user.Email;
                order.AppUserId = user.Id;

                ClearDbBasket(user.Id);

            }
            else
            {
                order.FullName = orderVM.FullName;
                order.Email = orderVM.Email;
                Response.Cookies.Delete("Basket");
            }
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("index", "home");


        }
        private void ClearDbBasket(string userId)
        {
            _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == userId).ToList());
            _context.SaveChanges();
        }
        private List<CheckoutItem> CheckoutItemsFromDb(string userId)
        {
            return _context.BasketItems.Include(x => x.Plant).Where(x => x.AppUserId == userId).Select(x => new CheckoutItem
            {
                PlantId = x.PlantId,
                Count = x.Count,
                Name = x.Plant.Name,
                Price = x.Plant.DiscountPercent > 0 ? ((x.Plant.SalePrice * (100 - x.Plant.DiscountPercent) / 100)) : x.Plant.SalePrice 

            }).ToList();
        }
        private List<CheckoutItem> CheckoutItemsFromCookie()
        {
            List<CheckoutItem> checkoutItems = new List<CheckoutItem>();
            var basketStr = Request.Cookies["basket"];
            if (basketStr != null)
            {
                List<BasketItemCookieViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
                foreach (var item in cookieItems)
                {
                    Plant plant = _context.Plants.FirstOrDefault(x => x.Id == item.PlantId);

                    CheckoutItem checkoutItem = new CheckoutItem()
                    {

                        Count = (int)item.Count,
                        PlantId = plant.Id,
                        Name = plant.Name,
                        Price = (decimal)(plant.DiscountPercent > 0 ? ((plant.SalePrice * (100 - plant.DiscountPercent) / 100) ) : plant.SalePrice)

                    };
                    checkoutItems.Add(checkoutItem);

                }

            }
            return checkoutItems;
        }

        private List<CheckoutItem> GetCheckoutItems()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return CheckoutItemsFromDb(userId);
            }
            else
            {
                return CheckoutItemsFromCookie();
            }
        }
    }
}
