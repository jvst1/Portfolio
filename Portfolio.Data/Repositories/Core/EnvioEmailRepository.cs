using Portfolio.Data.Queries.Core;
using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Core;
using Portfolio.Domain.Repositories.Core;

namespace Portfolio.Data.Repositories.Core
{
    public class EnvioEmailRepository : EntityFrameworkRepositoryBase<EnvioEmail>, IEnvioEmailRepository
    {
        public EnvioEmailRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }

        public List<EnvioEmail> GetAll(DateTime dtCorte, bool enviado, int limite)
        {
            var sql = new EnvioEmailQueries.GetAll(dtCorte, enviado, limite);
            return Context.RawQuery(sql).ToList();
        }
    }
}