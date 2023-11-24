using Portfolio.Data.Queries.Cad;
using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Repositories.Cad;

namespace Portfolio.Data.Repositories.Cad
{
    public class UsuarioEnderecoRepository : EntityFrameworkRepositoryBase<UsuarioEndereco>, IUsuarioEnderecoRepository
    {
        public UsuarioEnderecoRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<UsuarioEndereco> GetAll(string search, Guid codigoUsuario, bool? enderecoPrincipal = null)
        {
            return Context.RawQuery(new UsuarioEnderecoQueries.GetAll(search, codigoUsuario, enderecoPrincipal)).ToList();
        }
    }
}