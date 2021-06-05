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

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<PlayerAssessment> PlayerAssessments { get; set; }
        public DbSet<PlayerAttribute> PlayerAttributes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new MapMap());
            modelBuilder.ApplyConfiguration(new ArticleMap());
            modelBuilder.ApplyConfiguration(new PlayerAssessmentMap());
            modelBuilder.ApplyConfiguration(new PlayerAttributeMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
