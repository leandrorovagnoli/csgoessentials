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
            await CheckMapNameDuplicity(entity);
            return await _mapRepository.Add(entity);
        }

        public async Task Delete(Map entity)
        {
            await CheckMapHasRelationship(entity);
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

        public async Task<Map> GetByIdAsNoTrackingWithRelationship(int id)
        {
            return await _mapRepository.GetByIdAsNoTrackingWithRelationship(id);
        }

        public async Task Update(Map entity)
        {
            await CheckMapNameDuplicity(entity);
            await _mapRepository.Update(entity);
        }

        private async Task CheckMapNameDuplicity(Map entity)
        {
            var maps = await FindAsNoTracking(x => x.Name == entity.Name);
            var map = maps.FirstOrDefault();

            if (map != null && map.Id != entity.Id)
                throw new InvalidOperationException(Messages.MAPA_EXISTENTE);
        }

        private async Task CheckMapHasRelationship(Map entity)
        {
            var map = await GetByIdAsNoTrackingWithRelationship(entity.Id);

            if (map != null && map.Videos.Any())
                throw new InvalidOperationException(Messages.NAO_FOI_POSSIVEL_REMOVER_MAP_POSSUI_VIDEOS_CADASTRADOS);
        }
    }
}












