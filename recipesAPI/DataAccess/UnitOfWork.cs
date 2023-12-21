using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using recipesCommon.Interfaces;

namespace recipesApi.DataAccess
{
    public class UnitOfWork:IUnitOfWork
    {

        private Dictionary<System.Type, object> _repositories;
        private bool _disposed;
        DbContext Context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(RecipesDbContext context)
        {
            Context = context;
            _repositories = new();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }



        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {


            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }
        

            var repositoryInstance = new EFRepository<TEntity>(Context);
            _repositories.Add(typeof(TEntity), repositoryInstance);
            return repositoryInstance;
        }

        public async Task DeleteAllFromTable(string tableName)
        {
            await Context.Database.ExecuteSqlRawAsync("delete from " + tableName);
        }

        public Task SaveChangesAsync()
        {

            return Context.SaveChangesAsync();
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

    }
}
