using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Infrastructure.Helpers;
using System.Data;
using System.Linq.Expressions;
using Z.EntityFramework.Extensions;

namespace Portfolio.Data.Context
{
    public abstract class EfDbServiceBase : IDbService, IDisposable
    {
        private readonly EfContext _db;

        protected EfDbServiceBase(IConfiguration configuration, string identificador, CodigoUsuarioHelper codigoUsuarioHelper)
        {
            Console.WriteLine(configuration.GetSection("EnvironmentName").Value);

            _db = CreateContext(configuration, identificador, codigoUsuarioHelper);

            EntityFrameworkManager.ContextFactory =
                context =>
                {
                    if (context is EfContext efContext)
                    {
                        return CreateContext(configuration, efContext.Identificador, efContext.CodigoUsuarioHelper);
                    }

                    throw new NotSupportedException("Context não suportado.");
                };
        }

        private static EfContext CreateContext(IConfiguration configuration, string identificador, CodigoUsuarioHelper codigoUsuarioHelper)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("connectionString")); //todo: Set ConnectionString

            return new EfContext(optionsBuilder.Options, codigoUsuarioHelper, identificador);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetAll<TEntity>(bool nolock = false) where TEntity : class
        {
            if (nolock)
                SetNolock();

            return _db.Set<TEntity>().AsNoTracking();
        }

        /// <inheritdoc />
        public TEntity GetById<TEntity>(Guid id, bool nolock = false) where TEntity : class
        {
            if (nolock)
                SetNolock();

            var dbEntity = _db.Set<TEntity>().Find(id);

            if (dbEntity == null)
                return null;

            _db.Entry(dbEntity).State = EntityState.Detached;
            return dbEntity;
        }

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            _db.Set<TEntity>().Add(entity);
        }

        public void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _db.BulkInsert(entities);
        }

        public void BulkDelete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _db.BulkDelete(entities);
        }

        /// <inheritdoc />
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntry = _db.Entry(entity);
            dbEntry.State = EntityState.Modified;
        }

        /// <inheritdoc />
        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            var dbEntry = _db.Entry(entity);
            dbEntry.State = EntityState.Deleted;
        }

        public IQueryable<TResult> RawQuery<TResult>(IQuery<TResult> query) where TResult : class
        {
            var sql = query.GetSql();
            var parameters = ((IQuery<TResult, SqlParameter>)query).BuildParameterList();

            return _db.Set<TResult>().FromSqlRaw(sql, parameters).AsNoTracking().AsQueryable();
        }

        /// <inheritdoc />
        public int RawCommand(string sqlCommand, params object[] parameters)
        {
            return _db.Database.ExecuteSqlRaw(sqlCommand, parameters);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> filter, bool nolock = false) where TEntity : class
        {
            if (nolock)
                SetNolock();

            return _db.Set<TEntity>().Where(filter).AsNoTracking();
        }

        /// <inheritdoc />
        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, bool nolock = false) where TEntity : class
        {
            if (nolock) SetNolock();

            return _db.Set<TEntity>().AsNoTracking().SingleOrDefault(filter);
        }

        /// <inheritdoc />
        public bool HasAny<TEntity>(Expression<Func<TEntity, bool>> filter, bool nolock = false) where TEntity : class
        {
            if (nolock)
                SetNolock();

            return _db.Set<TEntity>().Any(filter);
        }

        /// <inheritdoc />
        public int Count<TEntity>() where TEntity : class
        {
            return _db.Set<TEntity>().Count();
        }

        public int SaveChanges()
        {
            var tracked = _db.ChangeTracker.Entries().ToList();
            var changes = _db.SaveChanges();

            foreach (var dbEntityEntry in tracked)
            {
                dbEntityEntry.State = EntityState.Detached;
            }

            return changes;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db?.Dispose();
            }
        }

        private void SetNolock()
        {
            const string sql = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

            _db.Database.ExecuteSqlRaw(sql);
        }

        public void BulkDelete<TEntity>(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
        }

        public void BulkDelete<TEntity>(string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
        }

        public void BulkUpdate<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
                Update(entity);
        }
    }
}
