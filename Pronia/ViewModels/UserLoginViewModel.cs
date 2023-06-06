using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email {get; set;}

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set;}
    }
}
