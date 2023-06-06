using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password {get; set;}
        [Required]
        [MaxLength(30)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set;}
        [Required]
        [MaxLength(100)]
        public string Email {get; set;}
        [Required]
        public string Token {get; set;}

        
    }
}
