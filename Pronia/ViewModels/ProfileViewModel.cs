using Pronia.Models;

namespace Pronia.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileEditViewModel Edit {get; set;}
        public List<Order> Orders { get; set;}
    }
}
