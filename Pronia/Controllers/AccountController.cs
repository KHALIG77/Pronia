using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProniaContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ProniaContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
           
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginViewModel userVM,string returnUrl=null)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email or Password incorrect");
            }

            AppUser user=await _userManager.FindByEmailAsync(userVM.Email);
            if(user == null ||user.IsAdmin==true) 
            {
                ModelState.AddModelError("", "Email or Password incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, false, false);
            if(!result.Succeeded) 
            {
                ModelState.AddModelError("", "Email or Password incorrect");
                return View();
            }
            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "home");

                 
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegisterVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(await _userManager.Users.AnyAsync(x=>x.UserName == userRegisterVm.UserName))
            {
                ModelState.AddModelError("UserName","Username already used");
                return View();
            }
            if (await _userManager.Users.AnyAsync(x => x.Email == userRegisterVm.Email))
            {
                ModelState.AddModelError("Email", "Email already used");
                return View();
            }
            AppUser user = new AppUser()
            {
                FullName = userRegisterVm.FullName,
                UserName = userRegisterVm.UserName,
                Email = userRegisterVm.Email,
                IsAdmin = false,
           };
            var result =await _userManager.CreateAsync(user, userRegisterVm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");

            return RedirectToAction("login");
        }
        
        

         
        
        
    }
}
