using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Manage.ViewModels;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly ProniaContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(ProniaContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
          
        }
        public IActionResult Index()
        {
           List<AppUser> admin = _context.AppUsers.Where(x=>x.IsAdmin==true).ToList();


            return View(admin);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateViewModel admin)
        {
            if (!ModelState.IsValid) return View(admin);
            if (admin.Password == null)
            {
                ModelState.AddModelError("Passwor", "Password is required");
                return View(admin);
            }
            if (_context.AppUsers.Any(x => x.UserName == admin.UserName))
            {
                ModelState.AddModelError("UserName", "Username already used");
                return View(admin);
            }

            AppUser newAdmin = new AppUser()
            {
                IsAdmin = true,
                PhoneNumber=admin.Phone,
                Email=admin.Email,
                FullName=admin.FullName,
                UserName=admin.UserName
                
            };
            var result = await _userManager.CreateAsync(newAdmin, admin.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(newAdmin);
            }
            await _userManager.AddToRoleAsync(newAdmin, "Admin");
            return RedirectToAction("index");
        }
       
        public IActionResult Delete(string id)
        {
            var admin=_userManager.FindByIdAsync(id).Result;
            if (admin==null)
            {
                return View();
            }
            var result =_userManager.DeleteAsync(admin).Result;
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            
            return Ok();
        }
        public IActionResult Edit(string Id)
        {
            AppUser admin = _userManager.FindByIdAsync(Id).Result;
            AdminCreateViewModel adminVM = new AdminCreateViewModel()
            {
                Email = admin.Email,
                UserName = admin.UserName,
                Phone = admin.PhoneNumber,
                FullName = admin.FullName,

            };
            ViewBag.Id=Id;

            return View(adminVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(AdminCreateViewModel adminVM)
        {
            ViewBag.Id = adminVM.Id;
            AppUser admin=_context.AppUsers.FirstOrDefault(x=>x.Id==adminVM.Id);
            if (admin == null) return View("Error");
            if (!ModelState.IsValid)
            {
                return View(adminVM);
            }
            if (admin.UserName!=adminVM.UserName && _context.AppUsers.Any(x=>x.UserName==adminVM.UserName))
            {
                ModelState.AddModelError("UserName", "Username already used");
                return View(adminVM);
            }
            if (adminVM.NewPassword!=null&& adminVM.Password!=null)
            {
              var result= _userManager.ChangePasswordAsync(admin, adminVM.Password, adminVM.NewPassword).Result;
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", "Current password is incorrect");
                        return View();
                    }
                }
            }
            admin.Email = adminVM.Email;
            admin.UserName = adminVM.UserName;
            admin.PhoneNumber = adminVM.Phone;
            admin.FullName = adminVM.FullName;

          var updateResult= _userManager.UpdateAsync(admin).Result;

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View(adminVM);
            }

            return RedirectToAction("index");
        }
    }

}
