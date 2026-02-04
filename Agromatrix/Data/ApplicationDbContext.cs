using Microsoft.EntityFrameworkCore;
using Agromatrix.Models;

namespace Agromatrix.Data
{
    // Application database context using Entity Framework Core with model configurations
    public class ApplicationDbContext : DbContext
    {
        // Constructor accepting DbContext options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DbSet for Products and Users
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.Name).HasColumnName("name");
                entity.Property(p => p.Description).HasColumnName("descr");
                entity.Property(p => p.Category).HasColumnName("categ");
                entity.Property(p => p.Price).HasColumnName("price");
                entity.Property(p => p.ImageUrl).HasColumnName("image_url");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Username).HasColumnName("username");
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.IsAdmin).HasColumnName("is_admin");
            });
        }
    }
}