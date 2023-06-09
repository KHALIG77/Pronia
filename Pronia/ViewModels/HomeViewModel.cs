using Pronia.Models;

namespace Pronia.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Slider { get; set; }
        public List<Feature> Feature { get; set; }
        public List<Plant>  FeaturedPlant { get; set; }
        public List<Plant> IsNewPlant {get; set; }
        public List<Plant> DiscountedPlant { get; set; }    
        public List<Plant> Rated {get; set; }
        public List<Banner> Banner { get; set; }


    }
}
