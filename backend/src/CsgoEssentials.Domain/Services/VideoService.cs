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

        public VideoService(IVideoRepository VideoRepository, IMapRepository MapRepository, IUserRepository UserRepository )
        {
            _videoRepository = VideoRepository;
            _mapRepository = MapRepository;
            _userRepository = UserRepository;
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

        public async Task<IEnumerable<Video>> FindAsNoTracking(Expression<Func<Video, bool>> predicate)
        {
            return await _videoRepository.FindAsNoTracking(predicate);
        }

        public async Task<IEnumerable<Video>> GetAll()
        {
            return await _videoRepository.GetAll();
        }

        public async Task<IEnumerable<Video>> GetAllAsNoTracking()
        {
            return await _videoRepository.GetAllAsNoTracking();
        }

        public async Task<Video> GetById(int id)
        {
            return await _videoRepository.GetById(id);
        }
        public async Task<Video> GetByIdAsNoTracking(int id)
        {
            return await _videoRepository.GetByIdAsNoTracking(id);
        }

        public async Task<Video> GetByIdAsNoTrackingWithRelationship(int id)
        {
            return await _videoRepository.GetByIdAsNoTrackingWithRelationship(id);
        }

        public async Task Update(Video entity)
        {
            await ReferencialIntegrityCheckUser(entity);
            await ReferencialIntegrityCheckMap(entity);
            await _videoRepository.Update(entity);
        }

        private async Task ReferencialIntegrityCheckUser(Video entity)
        {
            var users = await _userRepository.GetByIdAsNoTracking(entity.UserId);
            
            if (users == null)
                throw new InvalidOperationException(Messages.NOME_DE_USUARIO_JA_EXISTENTE);
        }

        private async Task ReferencialIntegrityCheckMap(Video entity)
        {
            var maps = await _mapRepository.GetByIdAsNoTracking(entity.MapId);
            
            if (maps == null)
                throw new InvalidOperationException(Messages.MAPA_EXISTENTE);
        }        
    }
}












