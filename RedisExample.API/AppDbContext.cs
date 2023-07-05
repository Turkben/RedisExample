using Microsoft.EntityFrameworkCore;
using RedisExample.API.Models;

namespace RedisExample.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Pen",
                    Price = 12.5m
                },
                new Product
                {
                    Id = 2,
                    Name = "Book",
                    Price = 50
                },
                new Product
                {
                    Id = 3,
                    Name = "Pencil",
                    Price = 7.25m
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
