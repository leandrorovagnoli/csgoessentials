using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Repository
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {

        }

        public override void Update(User entity)
        {
            var user = GetByIdAsNoTracking(entity.Id).Result;
            if (user != null && user.Password != entity.Password)
                entity.Password = MD5Hash.CalculaHash(entity.Password);

            base.Update(entity);
        }

        public override async Task<User> Add(User entity)
        {
            entity.Password = MD5Hash.CalculaHash(entity.Password);
            return await base.Add(entity);
        }
    }
}
