using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Pronia.Areas.Manage.ViewModels;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        UserName = "xaliq",
        //        IsAdmin = true,

        //    };

        //    var result = await _userManager.CreateAsync(admin, "xaliq123");
        //    await _userManager.AddToRoleAsync(admin, "Admin");
        //    return Json(result);

        //}
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));

        //    return Ok();
        //}



        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(AdminLoginViewModel adminVM,string returnUrl=null)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }
            AppUser admin = await _userManager.FindByNameAsync(adminVM.UserName);
            if(admin == null|| !admin.IsAdmin) 
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(admin, adminVM.Password, false, false);
            if(!result.Succeeded) 
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }
            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "dashboard");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
             return   RedirectToAction("login");
        }
    
    }
}
