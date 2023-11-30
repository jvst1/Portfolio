using Portfolio.Data.Queries.Cad;
using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Repositories.Cad;

namespace Portfolio.Data.Repositories.Cad
{
    public class CardapioRepository : EntityFrameworkRepositoryBase<Cardapio>, ICardapioRepository
    {
        public CardapioRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<Cardapio> GetAll(string search, Guid codigoComerciante)
        {
            return Context.RawQuery(new CardapioQueries.GetAll(search, codigoComerciante)).ToList();
        }

        public List<Cardapio> GetAllItensFromComerciante(Guid codigoComerciante)
        {
            return Context.RawQuery(new CardapioQueries.GetAllItensFromComerciante(codigoComerciante)).ToList();
        }

        public Cardapio GetItemByIdFromComerciante(Guid id, Guid codigoComerciante)
        {
            return Context.RawQuery(new CardapioQueries.GetItemByIdFromComerciante(id, codigoComerciante)).FirstOrDefault();
        }

        public List<Cardapio> GetOfertas()
        {
            return Context.RawQuery(new CardapioQueries.GetOfertas()).ToList();
        }
    }
}