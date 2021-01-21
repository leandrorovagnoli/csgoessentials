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

        public Task<Video> Add(Video entity)
        {
            return _videoRepository.Add(entity);
        }

        public void Delete(Video entity)
        {
            _videoRepository.Delete(entity);
        }

        public Task<IEnumerable<Video>> Find(Expression<Func<Video, bool>> predicate)
        {
            return _videoRepository.Find(predicate);
        }

        public Task<IEnumerable<Video>> GetAll()
        {
            return _videoRepository.GetAll();
        }

        public Task<IEnumerable<Video>> GetAllAsNoTracking()
        {
            return _videoRepository.GetAllAsNoTracking();
        }

        public Task<Video> GetById(int id)
        {
            return _videoRepository.GetById(id);
        }
        public Task<Video> GetByIdAsNoTracking(int id)
        {
            return _videoRepository.GetByIdAsNoTracking(id);
        }

        public void Update(Video entity)
        {
            _videoRepository.Update(entity);
        }
    }
}












