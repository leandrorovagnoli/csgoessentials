using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace CsgoEssentials.Infra.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Map> Maps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MapMap());

            base.OnModelCreating(modelBuilder);

        }
    }
}
