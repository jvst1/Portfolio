using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Domain.Repositories.Cad
{
    public interface ICategoriaRepository : ICrudRepository<Categoria>
    {
        List<Categoria> GetAll(string search);
    }
}
