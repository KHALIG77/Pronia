namespace Pronia.Models
{
    public class PlantTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int PlantId {get; set; }
        public Plant Plant { get; set; }
        public Tag Tag { get; set; }

    }
}
