using Microsoft.EntityFrameworkCore;
using recipesCommon.Interfaces;
using System.Linq.Expressions;

namespace recipesApi.DataAccess
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IRepository<TEntity> _repository;

        public IAsyncEnumerable<TEntity> GetAllAsyncStream(Expression<Func<TEntity, bool>> predicate) =>
            _repository.GetAllAsyncStream(predicate);

        public IQueryable<TEntity> Query()
        {
            return _repository.Query();
        }

        public Task<IEnumerable<TResult>> GetAllSelectedColumnsAsync<TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector)
        {
            return _repository.GetSelectedColumnsAsync(filter, selector);
        }

        public async Task<IEnumerable<TEntity>> GetAllIncludingAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = await _repository.GetAllIncludingAsync(includeProperties);
            return await query.Where(filter).ToListAsync();
        }

        public EntityService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) =>

            _repository.GetAllAsync(predicate);

        public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderByDescending = null)
        {

            return await _repository.GetOneAsync(predicate, orderByDescending);

        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            bool query = _repository.Any(predicate);
            return query;
        }


        public Task<IQueryable<TEntity>> GetAllAsync()
        {

            return _repository.GetAllAsync();
        }

        public IAsyncEnumerable<TEntity> GetAllAsAsyncEnumerable(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _repository.GetAllAsAsyncEnumerable(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.CountAsync(predicate);//.ConfigureAwait(false);
        }

        public async Task AddAsync(TEntity entity, bool saveChanges = true)
        {
            await _repository.AddAsync(entity).ConfigureAwait(false);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            await _repository.UpdateAsync(entity).ConfigureAwait(false);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            await _repository.UpdateRangeAsync(entities).ConfigureAwait(false);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            await _repository.AddRangeAsync(entities).ConfigureAwait(false);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(TEntity entity, bool saveChanges = true)
        {
            await _repository.DeleteAsync(entity).ConfigureAwait(false);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            await _repository.DeleteRangeAsync(entities);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
        }


        public async Task<TProperty> MaxAsync<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> selector)
        {
            return await _repository.MaxAsync(filter, selector).ConfigureAwait(false);//.ConfigureAwait(false);
        }
        public async Task<TProperty> MinAsync<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> selector)
        {
            return await _repository.MinAsync(filter, selector).ConfigureAwait(false);//.ConfigureAwait(false);
        }
    }
}
