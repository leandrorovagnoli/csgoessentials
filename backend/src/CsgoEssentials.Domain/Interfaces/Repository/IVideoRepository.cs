using CsgoEssentials.Domain.Entities;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Repository
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<Video> GetByIdAsNoTrackingWithRelationship(int id);
    }
}
