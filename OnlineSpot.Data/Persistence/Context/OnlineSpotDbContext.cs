using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OnlineSpot.Data.Domain.Entities;
using System.Diagnostics;



namespace OnlineSpot.Data.Persistence.Context
{
    public class OnlineSpotDbContext(DbContextOptions<OnlineSpotDbContext> options) : DbContext(options)
    {

        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineSpotDbContext).Assembly);
        }

    }
}
