
using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;

namespace CsgoEssentials.Infra.Repository
{
    public class ArticleRepository : EFRepository<Article>, IArticleRepository
    {
        public ArticleRepository(DataContext context) : base(context)
        {

        }
    }
}
