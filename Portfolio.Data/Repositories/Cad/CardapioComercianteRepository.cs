using Portfolio.Data.Queries.Cad;
using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Repositories.Cad;

namespace Portfolio.Data.Repositories.Cad
{
    public class CardapioComercianteRepository : EntityFrameworkRepositoryBase<CardapioComerciante>, ICardapioComercianteRepository
    {
        public CardapioComercianteRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<CardapioComerciante> GetAll(string search)
        {
            return Context.RawQuery(new CardapioComercianteQueries.GetAll(search)).ToList();
        }
    }
}