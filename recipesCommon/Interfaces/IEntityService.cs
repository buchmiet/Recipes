using System.Linq.Expressions;

namespace recipesCommon.Interfaces
{
    public interface IEntityService<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity, bool saveChanges = true);
        Task AddRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(TEntity entity, bool saveChanges = true);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
        IAsyncEnumerable<TEntity> GetAllAsAsyncEnumerable(Expression<Func<TEntity, bool>> predicate = null);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        IAsyncEnumerable<TEntity> GetAllAsyncStream(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllIncludingAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TResult>> GetAllSelectedColumnsAsync<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector);
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderByDescending = null);
        Task<TProperty> MaxAsync<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> selector);
        Task<TProperty> MinAsync<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> selector);
        IQueryable<TEntity> Query();
        Task UpdateAsync(TEntity entity, bool saveChanges = true);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
    }
}
