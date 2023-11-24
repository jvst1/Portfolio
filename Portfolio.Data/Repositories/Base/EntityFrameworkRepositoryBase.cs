using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Base.Interfaces.Domain;
using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Infrastructure.Enums;

namespace Portfolio.Data.Repositories.Base
{
    public abstract class EntityFrameworkRepositoryBase<TEntity> : ICrudRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly IDbService Context;

        public EntityFrameworkRepositoryBase(IDbProvider dbProvider)
        {
            var dbService = dbProvider.GetDbService<TEntity>(OrmEngine.Ef);

            Context = dbService;
        }

        #region Public Methods

        public virtual TEntity GetById(Guid id, bool nolock = false)
        {
            return Context.GetById<TEntity>(id, nolock);
        }

        public virtual void Insert(TEntity entity)
        {
            Context.Insert(entity);
        }

        public virtual void BulkInsert(IEnumerable<TEntity> entities)
        {
            Context.BulkInsert(entities);
        }

        public virtual void BulkDelete(IEnumerable<TEntity> entities)
        {
            Context.BulkDelete(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Remove(entity);
        }

        public virtual int Count()
        {
            return Context.Count<TEntity>();
        }

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Context?.Dispose();
        }

        public void BulkUpdate(IEnumerable<TEntity> entity)
        {
            Context.BulkUpdate(entity);
        }
    }
}
