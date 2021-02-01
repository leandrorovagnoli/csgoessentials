using CsgoEssentials.Domain.Entities;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Repository
{
    public interface IMapRepository : IRepository<Map>
    {
        Task<Map> GetByIdAsNoTrackingWithRelationship(int id);
    }
}
