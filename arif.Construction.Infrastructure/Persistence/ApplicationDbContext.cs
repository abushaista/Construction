using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EventData> Events { get; set; }
        public DbSet<ConstructionData> Constructions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventData>().HasKey(e => e.Id);
            modelBuilder.Entity<ConstructionData>().HasKey(e => e.Id);
            modelBuilder.Entity<ConstructionData>().HasIndex(e => e.UniqueId);
            modelBuilder.Entity<ConstructionData>().HasIndex(e => e.ProjectName);
            base.OnModelCreating(modelBuilder);
        }
    }
}
