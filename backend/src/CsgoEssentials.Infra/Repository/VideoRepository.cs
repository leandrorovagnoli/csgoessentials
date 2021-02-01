using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Repository
{
    public class VideoRepository : EFRepository<Video>, IVideoRepository
    {
        public VideoRepository(DataContext context) : base(context)
        {

        }

        public async Task<Video> GetByIdAsNoTrackingWithRelationship(int id)
        {
            return await _dbContext.Videos.Include(x => x.Map).Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
