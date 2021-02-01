using CsgoEssentials.Domain.Entities;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Services
{
    public interface IArticleService : IService<Article>
    {
        Task<Article> GetByIdAsNoTrackingWithRelationship(int id);
    }
}

