using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;

        public MapService(IMapRepository MapRepository)
        {
            _mapRepository = MapRepository;
        }
        public Task<Map> Add(Map entity)
        {
            return _mapRepository.Add(entity);
        }

        public void Delete(Map entity)
        {
            _mapRepository.Delete(entity);
        }

        public Task<IEnumerable<Map>> Find(Expression<Func<Map, bool>> predicate)
        {
            return _mapRepository.Find(predicate);
        }

        public Task<IEnumerable<Map>> GetAll()
        {
            return _mapRepository.GetAll();
        }

        public Task<IEnumerable<Map>> GetAllAsNoTracking()
        {
            return _mapRepository.GetAllAsNoTracking();
        }

        public Task<Map> GetById(int id)
        {
            return _mapRepository.GetById(id);
        }
        public Task<Map> GetByIdAsNoTracking(int id)
        {
            return _mapRepository.GetByIdAsNoTracking(id);
        }

        public void Update(Map entity)
        {
            _mapRepository.Update(entity);
        }
    }
}
