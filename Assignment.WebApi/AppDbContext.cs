using Assignment.WebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment.WebApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
    }
}
