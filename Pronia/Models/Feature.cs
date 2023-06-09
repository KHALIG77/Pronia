using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pronia.Attiributes.ValidationAttiributes;

namespace Pronia.Models
{
    public class Feature
    {
        public int Id { get; set; }
       
        [MaxLength(100)]
        public string IconUrl {get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg")]
        [FileSize(1000000)]
        public IFormFile ImageFile {get; set; }




    }
}
