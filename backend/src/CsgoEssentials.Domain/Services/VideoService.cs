using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IMapRepository _mapRepository;
        private readonly IUserRepository _userRepository;

        public VideoService(IVideoRepository videoRepository, IMapRepository mapRepository, IUserRepository userRepository )
        {
            _videoRepository = videoRepository;
            _mapRepository = mapRepository;
            _userRepository = userRepository;
        }

        public async Task<Video> Add(Video entity)
        {
            await ReferencialIntegrityCheckUser(entity);
            await ReferencialIntegrityCheckMap(entity);
            return await _videoRepository.Add(entity);
        }

        public async Task Delete(Video entity)
        {
            await _videoRepository.Delete(entity);
        }

        public async Task<IEnumerable<Video>> Find(Expression<Func<Video, bool>> predicate)
        {
            return await _videoRepository.Find(predicate);
        }
        public async Task<IEnumerable<Video>> GetAll()
        {
            return await _videoRepository.GetAll();
        }

        public async Task<Video> GetById(int id)
        {
            return await _videoRepository.GetById(id);
        }

        public async Task<Video> GetByIdWithRelationship(int id)
        {
            return await _videoRepository.GetByIdWithRelationship(id);
        }

        public async Task Update(Video entity)
        {
            await ReferencialIntegrityCheckUser(entity);
            await ReferencialIntegrityCheckMap(entity);
            await _videoRepository.Update(entity);
        }

        public async Task<IList<Video>> Filter(Query query)
        {
            return await _videoRepository.Filter(query);
        }

        private async Task ReferencialIntegrityCheckUser(Video entity)
        {
            var users = await _userRepository.GetById(entity.UserId);
            
            if (users == null)
                throw new InvalidOperationException(Messages.USUARIO_NAO_ENCONTRADO);
        }

        private async Task ReferencialIntegrityCheckMap(Video entity)
        {
            var maps = await _mapRepository.GetById(entity.MapId);
            
            if (maps == null)
                throw new InvalidOperationException(Messages.MAPA_EXISTENTE);
        }
    }
}












