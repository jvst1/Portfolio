using Portfolio.Infrastructure.Enums;

namespace Portfolio.Domain.Base.Interfaces.Data
{
    public interface IDbProvider
    {
        IDbService GetDbService(Type type, OrmEngine ormEngine);
        IDbService GetDbService<Type>(OrmEngine ormEngine);
    }
}
