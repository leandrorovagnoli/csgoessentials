using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;
        
        //constructor
        public ArticleService(IArticleRepository articleRepository, IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        public async Task<Article> Add(Article entity)
        {
            await CheckIfUserExists(entity);
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

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _articleRepository.GetAll();
        }

        public async Task<Article> GetByIdWithRelationship(int id)
        {
            return await _articleRepository.GetByIdWithRelationship(id);
        }

        public async Task<Article> GetById(int id)
        {
            return await _articleRepository.GetById(id);
        }

        public async Task Update(Article entity)
        {
            await CheckIfUserExists(entity);
            await _articleRepository.Update(entity);
        }

        private async Task CheckIfUserExists(Article entity)
        {
            var user = await _userRepository.GetById(entity.UserId);
            if (user == null)
                throw new InvalidOperationException(Messages.USUARIO_NAO_ENCONTRADO);
        }
    }
}
