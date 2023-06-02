using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;

namespace Pronia.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();	
		}
	}
}