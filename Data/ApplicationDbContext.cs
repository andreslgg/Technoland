using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using technoland.Models;

namespace Technoland.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Smartphones> Smartphones { get; set; }
        public DbSet<Technoland.Models.Invoices> Invoices { get; set; }
    }
}