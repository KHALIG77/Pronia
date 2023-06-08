using System.ComponentModel.DataAnnotations;
using Pronia.Enums;

namespace Pronia.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        public string Address {get; set; }
        public string Country {get; set; }
        [Required]
        [MaxLength(100)]
        public string Email {get; set; }
        public string Text {get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string AppUserId {get; set; }
        public AppUser AppUser { get; set; }
        public DateTime CreateAt {get; set; }
        public List<OrderItem> OrderItems { get; set; }=new List<OrderItem>();
    }
}
