using BoardGameShop.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Cấu hình Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Slug)
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Product>()
                .Property(p => p.ImageUrl)
                .HasMaxLength(255);

            modelBuilder.Entity<Product>()
                .Property(p => p.MetaTitle)
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.MetaDescription)
                .HasMaxLength(160);

            // Cấu hình Category
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<Category>()
                .Property(c => c.Slug)
                .HasMaxLength(50);

            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(255);

            // Cấu hình Order
            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerName)
                .HasMaxLength(100);

            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerPhone)
                .HasMaxLength(20);

            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerAddress)
                .HasMaxLength(255);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasMaxLength(50);

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMethod)
                .HasMaxLength(50);

            modelBuilder.Entity<Order>()
                .Property(o => o.CouponCode)
                .HasMaxLength(20);

            // Cấu hình User
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Phone)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.Address)
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasMaxLength(20);
        }
    }
}