using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName {get; set;}
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set;}
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set;}
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
