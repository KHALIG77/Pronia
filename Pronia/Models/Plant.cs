using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pronia.Attiributes.ValidationAttiributes;
using Pronia.Enums;

namespace Pronia.Models
{
    public class Plant
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName ="money")]
        public decimal SalePrice {get; set; }
        [Column(TypeName = "money")]
        public decimal CostPrice {get; set; }
        [Column(TypeName = "money")]
        public decimal DiscountPercent {get; set; }
        [Required]
        public bool StockStatus { get; set; }
        
        public byte Rate {get; set; }
        [Required]
        public PlantSize Size { get; set; }
        [Required]
        public bool isFeatured {get; set; }
        public bool isNew {get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<PlantTag> Tags { get; set; } = new List<PlantTag>();
        public List<PlantImage> Images { get; set; } = new List<PlantImage>();
        [NotMapped]
        public List<int> TagIds { get; set; }= new List<int>();
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg", "image/jpg")]
        [FileSize(1000000)]
        public IFormFile PosterImage {get; set; }
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg", "image/jpg")]
        [FileSize(1000000)]
        public IFormFile HoverImage { get; set; }
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg","image/jpg")]
        [FileSize(1000000)]
        public List<IFormFile> AllImages { get; set; }=new List<IFormFile>();
        [NotMapped]
        public List<int> ImageIds { get; set; } = new List<int>();

    }
}
