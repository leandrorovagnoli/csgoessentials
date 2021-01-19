﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Add(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> GetById(int id);
        Task<TEntity> GetByIdAsNoTracking(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsNoTracking();
        void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    }
}