using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;

namespace CsgoEssentials.Infra.Repository
{
    public class VideoRepository : EFRepository<Video>, IVideoRepository
    {
        public VideoRepository(DataContext context) : base(context)
        {

        }
    }
}
