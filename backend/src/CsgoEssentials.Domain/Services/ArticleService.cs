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

        public async Task<Article> Add(Article entity)
        {
            return await _articleRepository.Add(entity);
        }

        public async Task Delete(Article entity)
        {
            await _articleRepository.Delete(entity);
        }

        public async Task<IEnumerable<Article>> Find(Expression<Func<Article, bool>> predicate)
        {
            return await _articleRepository.Find(predicate);
        }

        public async Task<IEnumerable<Article>> FindAsNoTracking(Expression<Func<Article, bool>> predicate)
        {
            return await _articleRepository.FindAsNoTracking(predicate);
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _articleRepository.GetAll();
        }

        public async Task<IEnumerable<Article>> GetAllAsNoTracking()
        {
            return await _articleRepository.GetAllAsNoTracking();
        }

        public async Task<Article> GetById(int id)
        {
            return await _articleRepository.GetById(id);
        }

        public async Task<Article> GetByIdAsNoTracking(int id)
        {
            return await _articleRepository.GetByIdAsNoTracking(id);
        }

        public async Task Update(Article entity)
        {
            await _articleRepository.Update(entity);
        }
    }
}
