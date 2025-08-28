using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MolineMart.API.Models;

namespace MolineMart.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }    
        public DbSet<OrderItem> OrderItems { get; set; }
        = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
    }
}
