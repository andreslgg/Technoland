using NuGet.DependencyResolver;

namespace Technoland.Models
{
    public class InvoiceViewModel
    {
        public Invoices invoice { get; set; }
        public CheckoutViewModel listas { get; set; }
    }
}
