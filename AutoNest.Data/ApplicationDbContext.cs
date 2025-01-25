using AutoNest.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoNest.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Car> Cars { get;set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
