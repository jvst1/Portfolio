using Portfolio.Data.Queries.Cad;
using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Repositories.Cad;

namespace Portfolio.Data.Repositories.Cad
{
    public class UsuarioRepository : EntityFrameworkRepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<Usuario> GetAll(string search, string[] situacao)
        {
            return Context.RawQuery(new UsuarioQueries.GetAll(search, situacao)).ToList();
        }

        public List<Usuario> GetAllComerciantes()
        {
            return Context.RawQuery(new UsuarioQueries.GetAllComerciantes()).ToList();
        }

        public List<GuidStringRawQueryResponse> GetToSelect()
        {
            return Context.RawQuery(new UsuarioQueries.GetToSelect()).ToList();
        }

        public Usuario GetFirstByEmail(string email)
        {
            return Context.RawQuery(new UsuarioQueries.GetFirstByEmail(email)).FirstOrDefault();
        }

        public bool IsClientIdCadastrado(Guid codigoUsuario, string clientIdIdentityServer)
        {
            return Context.RawQuery(new UsuarioQueries.ClientIdCadastrado(codigoUsuario, clientIdIdentityServer)).Any();
        }

        public Usuario GetFirstByClientId(string clientIdIdentityServer)
        {
            return Context.RawQuery(new UsuarioQueries.GetFirstByClientId(clientIdIdentityServer)).FirstOrDefault();
        }

        public Usuario GetFirstByRefreshToken(string refreshToken)
        {
            return Context.RawQuery(new UsuarioQueries.GetFirstByRefreshToken(refreshToken)).FirstOrDefault();
        }

        public Usuario GetFirstByIdentificador(string identificador)
        {
            return Context.RawQuery(new UsuarioQueries.GetByIdentificador(identificador)).FirstOrDefault();
        }
    }
}