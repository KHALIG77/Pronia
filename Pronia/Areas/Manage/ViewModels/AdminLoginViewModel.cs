using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        public string Password { get; set; }    
    }
}
