using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using System;
using CsgoEssentials.Infra.Utils;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace CsgoEssentials.Domain.Services
{
    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;

        public MapService(IMapRepository MapRepository)
        {
            _mapRepository = MapRepository;
        }

        public async Task<Map> Add(Map entity)
        {
            await CheckUserNameDuplicity(entity);
            return await _mapRepository.Add(entity);
        }

        public async Task Delete(Map entity)
        {
            await _mapRepository.Delete(entity);
        }

        public async Task<IEnumerable<Map>> Find(Expression<Func<Map, bool>> predicate)
        {
            return await _mapRepository.Find(predicate);
        }

        public async Task<IEnumerable<Map>> FindAsNoTracking(Expression<Func<Map, bool>> predicate)
        {
            return await _mapRepository.FindAsNoTracking(predicate);
        }

        public async Task<IEnumerable<Map>> GetAll()
        {
            return await _mapRepository.GetAll();
        }

        public async Task<IEnumerable<Map>> GetAllAsNoTracking()
        {
            return await _mapRepository.GetAllAsNoTracking();
        }

        public async Task<Map> GetById(int id)
        {
            return await _mapRepository.GetById(id);
        }
        public async Task<Map> GetByIdAsNoTracking(int id)
        {
            return await _mapRepository.GetByIdAsNoTracking(id);
        }

        public async Task Update(Map entity)
        {
            await CheckUserNameDuplicity(entity);
            await _mapRepository.Update(entity);
        }

        private async Task CheckUserNameDuplicity(Map entity)
        {
            var maps = await FindAsNoTracking(x => x.Name == entity.Name);
            var map = maps.FirstOrDefault();

            if (map != null && map.Id != entity.Id)
                throw new InvalidOperationException(Messages.MAPA_EXISTENTE);
        }
        public async Task<Map> GetByIdAsNoTrackingWithVideos(int id)
        {
            return await _mapRepository.GetByIdAsNoTrackingWithVideos(id);
        }
    }
}












