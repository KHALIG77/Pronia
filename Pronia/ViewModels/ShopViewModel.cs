using Pronia.Enums;
using Pronia.Models;

namespace Pronia.ViewModels
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; } 
        public List<Tag> Tags { get; set; }
        public List<Plant> Plants { get; set; }
        public List<string> PlantSize { get; set; }
        public int AllPlantCount { get; set; }

    }
}
