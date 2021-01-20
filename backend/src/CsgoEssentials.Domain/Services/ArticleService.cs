using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        
        //constructor
        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public Task<Article> Add(Article entity)
        {
            return _articleRepository.Add(entity);
        }

        public void Delete(Article entity)
        {
            _articleRepository.Delete(entity);
        }

        public Task<IEnumerable<Article>> Find(Expression<Func<Article, bool>> predicate)
        {
            return _articleRepository.Find(predicate);
        }

        public Task<IEnumerable<Article>> GetAll()
        {
            return _articleRepository.GetAll();
        }

        public Task<IEnumerable<Article>> GetAllAsNoTracking()
        {
            return _articleRepository.GetAllAsNoTracking();
        }

        public Task<Article> GetById(int id)
        {
            return _articleRepository.GetById(id);
        }

        public Task<Article> GetByIdAsNoTracking(int id)
        {
            return _articleRepository.GetByIdAsNoTracking(id);
        }

        public void Update(Article entity)
        {
            _articleRepository.Update(entity);
        }
    }
}
