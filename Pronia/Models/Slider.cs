using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pronia.Attiributes.ValidationAttiributes;

namespace Pronia.Models
{
    public class Slider
    {
        public int Id {get;set;}
        public int Order {get;set;}
        public string BgUrl {get;set;}
        
        [MaxLength(100)]
        public string ImageUrl {get;set;}
        [MaxLength(30)]
        [Required]
        public string Offer {get;set;}
        [Required]
        [MaxLength(50)]
        
        public string Title { get;set;}
       
        public string Description { get;set;}

        [Required]
        public string ButtonUrl {get;set;}
        [Required]
        [MaxLength(30)]       
        
        public string ButtonText {get;set;}
        [NotMapped]
        [AllowFileType("image/png","image/jpeg")]
        [FileSize(1000000)]
        public IFormFile ImageFile {get;set;}
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg")]
        [FileSize(2000000)]
        public IFormFile BgImageFile { get;set;}    


    }
}
