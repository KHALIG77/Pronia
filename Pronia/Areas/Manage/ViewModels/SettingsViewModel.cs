using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pronia.Attiributes.ValidationAttiributes;

namespace Pronia.Areas.Manage.ViewModels
{
    public class SettingsViewModel
    {
        [MaxLength(60)]
        public string Address {get; set;}
        [MaxLength(60)]
        public string ContactPhone {get; set;}
        [MaxLength(60)]
        public string SupportPhone {get; set;}
        [NotMapped]
        [AllowFileType("image/jpg","image/jpeg","image/png")]
        [FileSize(1000000)]
        public IFormFile FileHeader {get; set;}
        [NotMapped]
        [AllowFileType("image/jpg", "image/jpeg", "image/png")]
        [FileSize(1000000)]
        public IFormFile FileFooter {get; set;}
        [MaxLength(100)]
        public string HeaderLogo {get; set;}
        [MaxLength(100)]
        public string FooterLogo {get; set;}
    }
}
