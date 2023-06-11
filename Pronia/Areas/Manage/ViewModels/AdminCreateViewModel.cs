using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Manage.ViewModels
{
    public class AdminCreateViewModel
    {
        public string Id {get; set;}
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }
       
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password {get; set;}
        [Required]
        [MaxLength(100)]
        public string UserName { get;set;}
        [MaxLength(20)]
        public string NewPassword {get; set;}
    }
}
