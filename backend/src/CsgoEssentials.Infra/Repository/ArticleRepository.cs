
using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Repository
{
    public class ArticleRepository : EFRepository<Article>, IArticleRepository
    {
        public ArticleRepository(DataContext context) : base(context)
        {

        }

        public async Task<Article> GetByIdAsNoTrackingWithRelationship(int id)
        {
            return await _dbContext.Articles.Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
