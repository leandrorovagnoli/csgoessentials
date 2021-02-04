using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CsgoEssentials.Infra.Repository
{
    public class VideoRepository : EFRepository<Video>, IVideoRepository
    {
        public VideoRepository(DataContext context) : base(context)
        {

        }

        public async Task<IList<Video>> Filter(Query query)
        {
            return await _dbContext.Videos
                .AsNoTracking()
                .Include(x => x.Map)
                .Include(x => x.User)
                .Where(x =>
                (!query.MapId.HasValue || x.MapId == query.MapId)
                && (!query.GrenadeType.HasValue || x.GrenadeType == query.GrenadeType)
                && (!query.TickRate.HasValue || x.TickRate == query.TickRate)
                && (!query.UserId.HasValue || x.UserId == query.UserId)).ToListAsync();
        }

        public async Task<Video> GetByIdWithRelationship(int id)
        {
            return await _dbContext.Videos.AsNoTracking().Include(x => x.Map).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
