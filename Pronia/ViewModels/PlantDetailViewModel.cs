using Pronia.Models;

namespace Pronia.ViewModels
{
    public class PlantDetailViewModel
    { 
        public Plant Plant {get; set;}
        public List<Feature> Features { get; set;}
        public PlantComment Comment { get;set;}
        public List<Plant> RelatedItems { get; set;}

    }
}
