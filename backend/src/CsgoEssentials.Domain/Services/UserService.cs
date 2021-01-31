using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<User> Add(User entity)
        {
            await CheckUserNameDuplicity(entity);
            await EncryptPassword(entity);
            
            return await _userRepository.Add(entity);
        }

        public async Task Delete(User entity)
        {
            await _userRepository.Delete(entity);
        }

        public async Task<IEnumerable<User>> Find(Expression<Func<User, bool>> predicate)
        {
            return await _userRepository.Find(predicate);
        }

        public async Task<IEnumerable<User>> FindAsNoTracking(Expression<Func<User, bool>> predicate)
        {
            return await _userRepository.FindAsNoTracking(predicate);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<IEnumerable<User>> GetAllAsNoTracking()
        {
            return await _userRepository.GetAllAsNoTracking();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }
        
        public async Task<User> GetByIdAsNoTracking(int id)
        {
            return await _userRepository.GetByIdAsNoTracking(id);
        }

        public async Task<User> GetByIdAsNoTrackingWithArticles(int id)
        {
            return await _userRepository.GetByIdAsNoTrackingWithArticles(id);
        }

        public async Task Update(User entity)
        {
            await EncryptPassword(entity);
            await CheckUserNameDuplicity(entity);
            await PreventUsernameChange(entity);

            await _userRepository.Update(entity);
        }

        private async Task EncryptPassword(User entity)
        {
            if (entity.Id == 0)
            {
                entity.Password = MD5Hash.CalculaHash(entity.Password);
                return;
            }

            //updating user
            var user = await GetByIdAsNoTracking(entity.Id);
            if (user != null && user.Password != entity.Password)
                entity.Password = MD5Hash.CalculaHash(entity.Password);
        }

        private async Task CheckUserNameDuplicity(User entity)
        {
            var users = await FindAsNoTracking(x => x.UserName == entity.UserName);
            var user = users.FirstOrDefault();

            if (user != null && user.Id != entity.Id)
                throw new InvalidOperationException(Messages.NOME_DE_USUARIO_JA_EXISTENTE);
        }

        private async Task PreventUsernameChange(User entity)
        {
            if (entity.Id == 0)
                return;

            var users = await FindAsNoTracking(x => x.Id == entity.Id);
            var user = users.FirstOrDefault();

            if (user != null && user.UserName != entity.UserName)
                throw new InvalidOperationException(Messages.NAO_E_PERMITIDO_ALTERAR_NOME_DE_USUARIO);
        }
    }
}
