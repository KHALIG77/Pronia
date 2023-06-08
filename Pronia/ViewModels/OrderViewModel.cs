namespace Pronia.ViewModels
{
    public class OrderViewModel
    {
        public List<CheckoutItem> Items { get; set; }
        public OrderFormViewModel OrderFormVM { get; set; }= new OrderFormViewModel();  
        public decimal TotalPrice { get; set; }
    }
}
