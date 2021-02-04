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

        public async Task<User> GetByIdWithRelationship(int id)
        {
            return await _dbContext.Users.AsNoTracking().Include(x => x.Articles).Include(x => x.Videos).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
