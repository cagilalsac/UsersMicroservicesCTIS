using CORE.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CORE.Services
{
    public abstract class DbService<TEntity> : Service, IDisposable where TEntity : Record, new()
    {
        private readonly DbContext _db;

        protected DbService(DbContext db)
        {
            _db = db;
        }

        protected virtual IQueryable<TEntity> DbQuery()
        {
            return _db.Set<TEntity>().AsNoTracking();
        }

        protected async Task<TEntity> DbSingleAsync(int id, CancellationToken cancellationToken = default)
            => await DbQuery().AsTracking().SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);


        protected async Task<TEntity> DbSingleAsync(Expression<Func<TEntity, bool>> predicate, 
            CancellationToken cancellationToken = default) 
            => await DbQuery().AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);

        protected virtual async Task<int> DbSaveAsync(CancellationToken cancellationToken = default)
            => await _db.SaveChangesAsync(cancellationToken);

        protected async Task DbAdd(TEntity entity, CancellationToken cancellationToken = default, 
            bool save = true)
        {
            _db.Set<TEntity>().Add(entity);
            if (save)
            {
                await DbSaveAsync(cancellationToken);
            }
        }

        protected async Task DbUpdate(TEntity entity, CancellationToken cancellationToken = default,
           bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
            {
                await DbSaveAsync(cancellationToken);
            }
        }

        protected async Task DbRemove(TEntity entity, CancellationToken cancellationToken = default,
          bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
            {
                await DbSaveAsync(cancellationToken);
            }
        }

        protected void DbRemove<TNavigationEntity>(List<TNavigationEntity> navigationEntities) 
            where TNavigationEntity : Record, new() 
        { 
            _db.Set<TNavigationEntity>().RemoveRange(navigationEntities); 
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
