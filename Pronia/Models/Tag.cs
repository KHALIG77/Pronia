using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string Name { get; set; }
        public List<PlantTag> Plants { get; set; }


    }
}
