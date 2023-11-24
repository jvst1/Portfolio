using Portfolio.Data.Queries.Cad;
using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Repositories.Cad;

namespace Portfolio.Data.Repositories.Cad
{
    public class CategoriaRepository : EntityFrameworkRepositoryBase<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<Categoria> GetAll(string search)
        {
            return Context.RawQuery(new CategoriaQueries.GetAll(search)).ToList();
        }
    }
}