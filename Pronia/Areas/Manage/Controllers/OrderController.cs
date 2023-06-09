using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    public class OrderController : Controller
    {
        private readonly ProniaContext _context;

        public OrderController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
