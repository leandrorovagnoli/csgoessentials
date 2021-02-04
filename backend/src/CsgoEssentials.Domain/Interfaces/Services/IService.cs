using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<TEntity> Add(TEntity entity);
        Task Update(TEntity entity);
        Task<IEnumerable<TEntity>> GetAll();
        Task Delete(TEntity entity);
        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
