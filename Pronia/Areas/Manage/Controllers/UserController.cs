using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly ProniaContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(ProniaContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public  IActionResult Index(int page=1,string search=null)
        {
            var query =  _context.AppUsers.Where(x=>x.IsAdmin==false).AsQueryable();
            if (search != null)
            {
                query=query.Where(x=>x.FullName.Contains(search));
            }
            return View(PaginatedList<AppUser>.Create(query,page,4));
        }
       
           
        
        
       
    }
}
