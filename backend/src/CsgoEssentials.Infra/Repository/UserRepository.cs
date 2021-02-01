using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Repository
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {

        }

        public async Task<User> GetByIdAsNoTrackingWithArticles(int id)
        {
            return await _dbContext.Users.Include(x => x.Articles).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User> GetByIdAsNoTrackingWithUserVideos(int id)
        {
            return await _dbContext.Users.Include(x => x.Videos).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
