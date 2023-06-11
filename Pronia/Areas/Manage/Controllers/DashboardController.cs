using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Manage.ViewModels;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]

    public class DashboardController : Controller
    {
        private readonly ProniaContext _context;

        public DashboardController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime dateTime = DateTime.Now;
            int currentMonth=dateTime.Month;
            int currentYear = dateTime.Year;

            var data =_context.Orders.Include(x=>x.OrderItems).AsQueryable();
            var monthlyData = data.Where(x => x.CreateAt.Month == currentMonth && x.CreateAt.Year == currentYear).ToList();
            var monthlyTotal = monthlyData.Sum(order => order.OrderItems.Sum(orderItem => orderItem.UnitPrice * orderItem.Count));

            var yearlyData = data.Where(order => order.CreateAt.Year == currentYear).ToList();
            var yearlyTotal = yearlyData.Sum(order => order.OrderItems.Sum(orderItem => orderItem.UnitPrice * orderItem.Count));

            var pendingStatus = data.Where(order => order.OrderStatus == Enums.OrderStatus.Pending).Count();

            MonthViewModel months = new MonthViewModel()
            {
                Jan = MonthData(data.ToList(), 1),
                Feb = MonthData(data.ToList(), 2),
                Mar = MonthData(data.ToList(), 3),
                Apr = MonthData(data.ToList(), 4),
                May = MonthData(data.ToList(), 5),
                Jun = MonthData(data.ToList(), 6),
                Jul = MonthData(data.ToList(), 7),
                Avg = MonthData(data.ToList(), 8),
                Sep = MonthData(data.ToList(), 9),
                Okt = MonthData(data.ToList(), 10),
                Noy = MonthData(data.ToList(), 11),
                Dec = MonthData(data.ToList(), 12),

            };
            PieChartViewModel pie = new PieChartViewModel()
            {
                AccessPercent = Math.Round((((decimal)data.Where(order=>order.OrderStatus==Enums.OrderStatus.Access).Count()/(decimal)data.Count())*100)),
                PendingPercent = Math.Round((((decimal)data.Where(order => order.OrderStatus == Enums.OrderStatus.Pending).Count() / (decimal)data.Count()) * 100)),
                RejecPercent = Math.Round((((decimal)data.Where(order => order.OrderStatus == Enums.OrderStatus.Rejected).Count() / (decimal)data.Count()) * 100)),


            };
           




            ChartsDataViewModel vm = new ChartsDataViewModel()
            {
                MonthlyEarnings = monthlyTotal,
                YearlyEarnings = yearlyTotal,
                PendingOrderCount = pendingStatus,
                Months = months,
                PieChart = pie


            };
            return View(vm);
        }

        public static int MonthData(List<Order> query,int value)
        {
            var monthlyData = query.Where(x => x.CreateAt.Month == value).ToList();
            var monthlyTotal = monthlyData.Sum(order => order.OrderItems.Sum(orderItem => orderItem.UnitPrice * orderItem.Count));
            return   (int)monthlyTotal;
        }
    }
}
