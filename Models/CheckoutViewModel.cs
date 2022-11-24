
using technoland.Models;
using Technoland.Data;

namespace Technoland.Models
{
    public class CheckoutViewModel
    {
      
        public List<Smartphones> CartProducts { get; set; }
        public List<int> CartProductIDs { get; set; }   

        public double total { get; set; }    
    }
}
