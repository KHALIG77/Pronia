using Microsoft.AspNetCore.Mvc;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]

    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
