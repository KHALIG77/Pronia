namespace Pronia.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> BasketItems {get; set;}=new List<BasketItemViewModel>();
        public byte AllCount { get; set;} 
        public decimal TotalPrice {get; set;}
    }
}
