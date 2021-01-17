using CsgoEssentials.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CsgoEssentials.Infra.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
