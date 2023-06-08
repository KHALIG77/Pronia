using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class OrderFormViewModel
    {
        [MaxLength(100)]    
        public string FullName {get; set;}
        [MaxLength(100)]
        public string Address {get; set;}
        [MaxLength(100)]
        public string Country { get; set;}
        [MaxLength(100)]
        public string Email { get; set;}
        [MaxLength(300)]
        public string Text { get; set;}

        
    }
}
