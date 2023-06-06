using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        public string IconUrl {get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
