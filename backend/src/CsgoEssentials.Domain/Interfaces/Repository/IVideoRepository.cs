using CsgoEssentials.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Repository
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<Video> GetByIdWithRelationship(int id);

        Task<IList<Video>> Filter(Query query);
    }
}
