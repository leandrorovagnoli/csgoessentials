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
        public async Task<Map> GetByIdAsNoTrackingWithRelationship(int id)
        {
            return await _dbContext.Maps.Include(x => x.Videos).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
