using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Domain.Repositories.Cad
{
    public interface ICardapioRepository : ICrudRepository<Cardapio>
    {
        List<Cardapio> GetAll(string search, Guid codigoComerciante);
        List<Cardapio> GetAllItensFromComerciante(Guid codigoComerciante);
        Cardapio GetItemByIdFromComerciante(Guid id, Guid codigoComerciante);
        List<Cardapio> GetOfertas();
    }
}
