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

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<RelatedProduct> RelatedProducts { get; set; }
        public DbSet<DeliveryAvailability> DeliveryAvailability { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature { Id = 1, ProductId = 1, FeatureTitle = "Processor", FeatureDescription = "A16 Bionic Chip" },
                new ProductFeature { Id = 2, ProductId = 1, FeatureTitle = "Display", FeatureDescription = "6.1-inch Super Retina XDR Display" },
                new ProductFeature { Id = 3, ProductId = 1, FeatureTitle = "Camera", FeatureDescription = "48MP main camera with cinematic mode" },
                new ProductFeature { Id = 4, ProductId = 1, FeatureTitle = "Battery", FeatureDescription = "Up to 24 hours video playback" }
            );

            modelBuilder.Entity<ProductReview>().HasData(
                new ProductReview { Id = 1, ProductId = 1, UserName = "Amit Sharma", Rating = 5, ReviewText = "Great phone!" },
                new ProductReview { Id = 2, ProductId = 1, UserName = "Neha Gupta", Rating = 4, ReviewText = "Excellent but expensive." }, 
                new ProductReview { Id = 3, ProductId = 2, UserName = "Rahul Verma", Rating = 5, ReviewText = "S Pen is absolutely amazing." }, 
                new ProductReview { Id = 4, ProductId = 2, UserName = "Priya Singh", Rating = 4, ReviewText = "Battery life could be better." }, 
                new ProductReview { Id = 5, ProductId = 3, UserName = "Karan Malik", Rating = 5, ReviewText = "Fast performance and super smooth UI." }, 
                new ProductReview { Id = 6, ProductId = 4, UserName = "Anil Kumar", Rating = 3, ReviewText = "Great software experience – clean and intuitive." } 
            );

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "/images/iphone14pro_front.jpg" },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "/images/iphone14pro_back.jpg" },
                new ProductImage { Id = 3, ProductId = 1, ImageUrl = "/images/iphone14pro_side.jpg" },
                new ProductImage { Id = 4, ProductId = 2, ImageUrl = "/images/galaxys23ultra_front.jpg" },
                new ProductImage { Id = 5, ProductId = 2, ImageUrl = "/images/galaxys23ultra_back.jpg" },
                new ProductImage { Id = 6, ProductId = 2, ImageUrl = "/images/galaxys23ultra_side.jpg" },
                new ProductImage { Id = 7, ProductId = 3, ImageUrl = "/images/oneplus11r_front.jpg" },
                new ProductImage { Id = 8, ProductId = 3, ImageUrl = "/images/oneplus11r_back.jpg" },
                new ProductImage { Id = 9, ProductId = 4, ImageUrl = "/images/pixel8_front.jpg" },
                new ProductImage { Id = 10, ProductId = 4, ImageUrl = "/images/pixel8_back.jpg" },
                new ProductImage { Id = 11, ProductId = 4, ImageUrl = "/images/pixel8_side.jpg" }
            );

            modelBuilder.Entity<RelatedProduct>().HasData(
                new RelatedProduct { Id = 1, ProductId = 1, RelatedProductId = 3 },
                new RelatedProduct { Id = 2, ProductId = 1, RelatedProductId = 4 }, 
                new RelatedProduct { Id = 3, ProductId = 2, RelatedProductId = 3 }, 
                new RelatedProduct { Id = 4, ProductId = 2, RelatedProductId = 5 }
            );

            // ✅ Seed DeliveryAvailability data
            modelBuilder.Entity<DeliveryAvailability>().HasData(
                new DeliveryAvailability { Id = 1, Pincode = "110001", IsAvailable = true, EstimatedDays = 2 },
                new DeliveryAvailability { Id = 2, Pincode = "400001", IsAvailable = true, EstimatedDays = 3 },
                new DeliveryAvailability { Id = 3, Pincode = "560001", IsAvailable = false, EstimatedDays = 2 }
            );

        }
    }
}
