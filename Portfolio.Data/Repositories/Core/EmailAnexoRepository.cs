using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Core;
using Portfolio.Domain.Repositories.Core;

namespace Portfolio.Data.Repositories.Core
{
    public class EmailAnexoRepository : EntityFrameworkRepositoryBase<EnvioEmailAnexo>, IEmailAnexoRepository
    {
        public EmailAnexoRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<EnvioEmailAnexo> GetAllByCodigoEmail(Guid emailCodigo)
        {
            return Context.Query<EnvioEmailAnexo>(e => e.CodigoEnvioEmail == emailCodigo).ToList();
        }
    }
}