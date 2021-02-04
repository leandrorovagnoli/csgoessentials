using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Repository
{
    public class MapRepository : EFRepository<Map>, IMapRepository
    {
        public MapRepository(DataContext context) : base(context)
        {

        }
        public async Task<Map> GetByIdWithRelationship(int id)
        {
            return await _dbContext.Maps.AsNoTracking().Include(x => x.Videos).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
