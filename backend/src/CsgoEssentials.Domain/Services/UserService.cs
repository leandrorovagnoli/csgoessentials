using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
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
        public Task<User> Add(User entity)
        {
            return _userRepository.Add(entity);
        }

        public void Delete(User entity)
        {
            _userRepository.Delete(entity);
        }

        public Task<IEnumerable<User>> Find(Expression<Func<User, bool>> predicate)
        {
            return _userRepository.Find(predicate);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return _userRepository.GetAll();
        }

        public Task<IEnumerable<User>> GetAllAsNoTracking()
        {
            return _userRepository.GetAllAsNoTracking();
        }

        public Task<User> GetById(int id)
        {
            return _userRepository.GetById(id);
        }
        public Task<User> GetByIdAsNoTracking(int id)
        {
            return _userRepository.GetByIdAsNoTracking(id);
        }

        public void Update(User entity)
        {
            _userRepository.Update(entity);
        }
    }
}
