using Microsoft.Extensions.Configuration;
using Portfolio.Infrastructure.Attributes;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Data.Context.DbServices
{
    [IdentificadorDbService(Identificadores.Portfolio, OrmEngine.Ef)]
    public class PortfolioEfDbService : EfDbServiceBase
    {
        public PortfolioEfDbService(IConfiguration configuration, CodigoUsuarioHelper codigoUsuarioHelper) : base(configuration, Identificadores.Portfolio, codigoUsuarioHelper)
        {
        }
    }
}
