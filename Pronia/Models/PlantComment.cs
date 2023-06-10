using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class PlantComment
    {
        public int Id { get; set; }
        public string AppUserId {get; set;}
        public int PlantId {get; set;}
        [Range(1,5)]
        [Required]
        public int Rate {get; set;}
        [MaxLength(400)]
        [Required]
        public string Comment {get; set;}
        public string ReplyComment {get; set;}
        public DateTime ReplyTime {get; set;}
        public DateTime CreatedAt {get; set;}
        public AppUser AppUser {get; set;}  
        public Plant Plant { get; set; }

    }
}
