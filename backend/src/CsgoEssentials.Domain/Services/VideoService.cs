using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;

        public VideoService(IVideoRepository VideoRepository)
        {
            _videoRepository = VideoRepository;
        }

        public async Task<Video> Add(Video entity)
        {
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

        public async Task Update(Video entity)
        {
            await _videoRepository.Update(entity);
        }
    }
}












