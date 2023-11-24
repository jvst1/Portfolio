using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;

namespace Portfolio.Domain.Repositories.Cad
{
    public interface IUsuarioEnderecoRepository : ICrudRepository<UsuarioEndereco>
    {
        List<UsuarioEndereco> GetAll(string search, Guid codigoUsuario, bool? enderecoPrincipal);
    }
}
