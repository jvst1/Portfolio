using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;

namespace Portfolio.Domain.Repositories.Cad
{
    public interface IUsuarioRepository : ICrudRepository<Usuario>
    {
        List<Usuario> GetAll(string search, string[] situacao);
        List<GuidStringRawQueryResponse> GetToSelect();
        bool IsClientIdCadastrado(Guid codigoUsuario, string clientIdIdentityServer);
        Usuario GetFirstByClientId(string clientIdIdentityServer);
        Usuario GetFirstByEmail(string email);
        Usuario GetFirstByRefreshToken(string refreshToken);
        Usuario GetFirstByIdentificador(string identificador);
    }
}
