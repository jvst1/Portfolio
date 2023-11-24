using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Domain.Repositories.Cad
{
    public interface ICardapioComercianteRepository : ICrudRepository<CardapioComerciante>
    {
        List<CardapioComerciante> GetAll(string search);
    }
}
