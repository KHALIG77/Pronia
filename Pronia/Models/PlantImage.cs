using Pronia.Enums;

namespace Pronia.Models
{
    public class PlantImage
    {
        public int Id {get; set;}
        public string ImageName {get; set;}
        public int PlantId {get; set;}
        public ImageStatus ImageStatus { get; set;}
        public Plant Plant { get; set;}
    }
}
