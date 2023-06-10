using Microsoft.AspNetCore.Mvc;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    public class PlantComment : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
