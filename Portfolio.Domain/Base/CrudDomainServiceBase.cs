using Portfolio.Domain.Base.Interfaces.Domain;
using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Domain.Base
{
    public abstract class CrudDomainServiceBase<TRepository, TEntity> where TRepository : ICrudRepository<TEntity> where TEntity : IEntity
    {
        protected readonly TRepository CrudRepository;

        protected CrudDomainServiceBase(TRepository crudRepository)
        {
            CrudRepository = crudRepository;
        }

        public virtual TEntity GetById(Guid id, bool nolock = false)
        {
            return CrudRepository.GetById(id, nolock);
        }

        public virtual void Insert(TEntity entity)
        {
            entity.ThrowExceptionIfInsertInvalid();

            CrudRepository.Insert(entity);
            CrudRepository.SaveChanges();
        }

        public virtual void BulkInsert(IEnumerable<TEntity> entity)
        {
            entity.SafeForeach(p => p.ThrowExceptionIfInsertInvalid());

            CrudRepository.BulkInsert(entity);
            CrudRepository.SaveChanges();
        }
        public virtual void BulkDelete(IEnumerable<TEntity> entity)
        {
            entity.SafeForeach(p => p.ThrowExceptionIfInsertInvalid());

            CrudRepository.BulkDelete(entity);
            CrudRepository.SaveChanges();
        }

        public virtual void Update(Guid id, TEntity entity)
        {
            entity.ThrowExceptionIfUpdateInvalid();

            var current = CrudRepository.GetById(id, true);

            if (current == null)
                throw new ArgumentException($"Não foi possível encontrar uma entidade com o ID {id} para realizar a atualização. Certifique-se de que o ID está correto e tente novamente.");

            // Atualiza os dados que podem ser alterados
            current.UpdateAllProperties(entity);

            // Atualiza o objeto, incluindo os dados que podem estar faltando
            entity.UpdateAllProperties(current, false);

            CrudRepository.Update(current);
            CrudRepository.SaveChanges();
        }

        public virtual void Remove(Guid id)
        {
            var entity = GetById(id);

            if (entity == null)
                throw new ArgumentException($"Não foi possível encontrar uma entidade com o ID {id} para remoção. Por favor, verifique se o ID é válido e tente novamente.");

            CrudRepository.Remove(entity);
            CrudRepository.SaveChanges();
        }
    }
}
