﻿using CsgoEssentials.Domain.Entities;
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

        public async Task<Map> Add(Map entity)
        {
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
            await _mapRepository.Update(entity);
        }
    }
}












