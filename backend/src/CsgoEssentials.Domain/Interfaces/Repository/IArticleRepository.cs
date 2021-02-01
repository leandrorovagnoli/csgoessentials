using CsgoEssentials.Domain.Entities;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Repository
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article> GetByIdAsNoTrackingWithRelationship(int id);
    }
}


