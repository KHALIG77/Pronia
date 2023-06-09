using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pronia.Attiributes.ValidationAttiributes;

namespace Pronia.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string ImageUrl {get; set;}
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg")]
        [FileSize(1000000)]
        public IFormFile ImageFile { get; set;}
    }
}
