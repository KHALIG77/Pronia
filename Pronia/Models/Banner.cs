using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pronia.Attiributes.ValidationAttiributes;

namespace Pronia.Models
{
    public class Banner
    {
        public int Id { get; set; }
        [NotMapped]
        [AllowFileType("image/jpg","image/jpeg","image/png")]
        [FileSize(1000000)]
        public IFormFile ImageFile {get; set;}
        [Required]
        [MaxLength(60)]
        public string Title {get; set;}
        [Required]
        [MaxLength(150)]
        public string Description { get; set;}
        [Required]
        public string ButtonUrl { get; set; }
        [Required]
        [MaxLength(30)]
        public string ButtonText { get; set; }
        [MaxLength(100)]
        public string ImageUrl {get; set;}


    }
}
