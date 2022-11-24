using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using technoland.Models;

namespace Technoland.Models
{
    public class Invoices
    {
        [Key]
        public Guid Id { get; set; }
        public string Cart { get; set; }
        public DateTime CreatedDate { get; set; }

        public double Total { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual IdentityUser? User { get; set; }

    }
}
