using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;

namespace CsgoEssentials.Infra.Repository
{
    public class MapRepository : EFRepository<Map>, IMapRepository
    {
        public MapRepository(DataContext context) : base(context)
        {

        }
    }
}
