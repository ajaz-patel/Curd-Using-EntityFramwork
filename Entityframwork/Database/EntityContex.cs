using Entityframwork.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace Entityframwork.Database
{
    public class EntityContex:DbContext
    {
        public readonly IConfiguration _cofig;
        
        public EntityContex(IConfiguration config)
        {
            _cofig = config;
        }
        public virtual DbSet<Usermodel> Usertab { get; set; } 

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_cofig.GetConnectionString("DefaultConnection"),
                     optionsBuilder => optionsBuilder.EnableRetryOnFailure()
                    );
            }
        }
        protected override  void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Dotnet");
            modelBuilder.Entity<Usermodel>()
                .ToTable("Usertab", "Dotnet")
                .HasKey(u=> u.UserId);
        }


    }
}
