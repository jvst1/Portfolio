using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Base.Interfaces.Domain;
using System.Linq.Expressions;

namespace Portfolio.Data.DbMappings.Base
{
    public abstract class DbMappingBase<T> : IDbMapping<T> where T : IEntity
    {
        public abstract string DbService { get; }
        public abstract string Schema { get; }
        public virtual bool SaveTrail => true;
        public virtual Expression<Func<IEntity, Guid>> GetCodigo() => entity => entity.Codigo;
    }
}
