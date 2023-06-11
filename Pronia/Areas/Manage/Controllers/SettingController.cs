using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Manage.ViewModels;
using Pronia.DAL;
using Pronia.Helper.FileManager;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize("SuperAdmin,Admin")]

    public class SettingController : Controller
    {
        private readonly ProniaContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(ProniaContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {

            SettingsViewModel setting = new SettingsViewModel()
            {
                Address = _context.Settings.FirstOrDefault(x => x.Key == "Address").Value,
                ContactPhone = _context.Settings.FirstOrDefault(x => x.Key == "ContactPhone").Value,
                SupportPhone = _context.Settings.FirstOrDefault(x => x.Key == "SupportPhone").Value,
                HeaderLogo = _context.Settings.FirstOrDefault(x => x.Key == "HeaderLogo").Value,
                FooterLogo = _context.Settings.FirstOrDefault(x => x.Key == "FooterLogo").Value,

            };
            return View(setting);
           
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string oldHeaderName = null;
            if (model.FileHeader!=null)
            {
                if(model.HeaderLogo!=null)
                {
                    oldHeaderName = model.HeaderLogo;
                }
                model.HeaderLogo = FileManager.Save(_env.WebRootPath, "uploads/settings", model.FileHeader);
            }
            string oldFooterName = null;
            if (model.FileFooter != null)
            {
                if (model.FileFooter != null)
                {
                    oldFooterName = model.FooterLogo;
                }
                model.FooterLogo = FileManager.Save(_env.WebRootPath, "uploads/settings", model.FileFooter);
            }
            if (model.FileHeader == null&& model.HeaderLogo == null)
            {
                var check = _context.Settings.FirstOrDefault(x => x.Key =="HeaderLogo")?.Value;
                if (check != null)
                {
                    oldHeaderName = check;
                }
            }
            if (model.FileHeader == null && model.FooterLogo == null)
            {
                var check = _context.Settings.FirstOrDefault(x => x.Key == "FooterLogo")?.Value;
                if (check != null)
                {
                    oldFooterName = check;
                }
            }
            _context.Settings.FirstOrDefault(x => x.Key == "Address").Value = model.Address;
            _context.Settings.FirstOrDefault(x => x.Key == "ContactPhone").Value = model.ContactPhone;
            _context.Settings.FirstOrDefault(x => x.Key == "SupportPhone").Value = model.SupportPhone;
            _context.Settings.FirstOrDefault(x => x.Key == "HeaderLogo").Value = model.HeaderLogo;
            _context.Settings.FirstOrDefault(x => x.Key == "FooterLogo").Value = model.FooterLogo;

            _context.SaveChanges();


            if (oldFooterName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/settings", oldFooterName);
            }
            if (oldHeaderName!=null){
                FileManager.Delete(_env.WebRootPath, "uploads/settings", oldHeaderName);

            }


            return RedirectToAction("index");
        }
    }
}
