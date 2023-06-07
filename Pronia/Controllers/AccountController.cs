using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Services;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProniaContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(ProniaContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IEmailSender emailSender)
        {
           
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetVM)
        {
            if (!ModelState.IsValid) { return View(); };
            AppUser user = await _userManager.FindByEmailAsync(forgetVM.Email);
            if(user== null||user.IsAdmin) { ModelState.AddModelError("Email", "Email not found"); };
            string token =await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = Url.Action("resetpassword", "account", new {email=forgetVM.Email,token=token},Request.Scheme);
            _emailSender.Send(forgetVM.Email, "Reset Password", $" Click <a href=\"{url}\"> Here</a>");

            return RedirectToAction("login");
        }
        public async Task<IActionResult> ResetPassword(string email,string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user==null||user.IsAdmin||!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                ModelState.AddModelError("Email", "Email not found");
            }
            ViewBag.Email = email;
            ViewBag.Token = token;  
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetVM)
        {
            ViewBag.Email = resetVM.Email;
            ViewBag.Token=resetVM.Token;

            AppUser user = await _userManager.FindByEmailAsync(resetVM.Email);

            if (user == null||user.IsAdmin)
            { ModelState.AddModelError("Email", "Email not found");return View(); }
            var result =await _userManager.ResetPasswordAsync(user, resetVM.Token, resetVM.Password);
            if (!result.Succeeded) 
            {
                ModelState.AddModelError("","Write correctly");
                return View();
            }
            return RedirectToAction("login");
        }
        public IActionResult GoogleLogin()
        {
            string url = Url.Action("googleresponse", "account", Request.Scheme);
            var prop = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return new ChallengeResult("Google",prop);

        }
        public async Task<IActionResult> GoogleResponse()
        {
            var info = _signInManager?.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("login");

            }
            var email = info.Result.Principal.FindFirstValue(ClaimTypes.Email);
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser()
                {
                    Email = email,
                    UserName = email

                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return RedirectToAction("login");
                }
                await _userManager.AddToRoleAsync(user, "Member");
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("index", "home");
        }
    }

}

