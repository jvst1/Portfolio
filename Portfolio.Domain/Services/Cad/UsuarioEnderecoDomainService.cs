using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Repositories.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Domain.Services.Cad
{
    public class UsuarioEnderecoDomainService : CrudDomainServiceBase<IUsuarioEnderecoRepository, UsuarioEndereco>
    {
        private readonly AppSettings _appSettings;
        private ILogger<UsuarioEnderecoDomainService> _logger;

        public UsuarioEnderecoDomainService(IOptions<AppSettings> appSettings,
                                    ILogger<UsuarioEnderecoDomainService> logger,
                                    IUsuarioEnderecoRepository crudRepository) : base(crudRepository)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public List<UsuarioEndereco> GetAll(string search, Guid codigoUsuario, bool? enderecoPrincipal)
        {
            return CrudRepository.GetAll(search, codigoUsuario, enderecoPrincipal);
        }
    }
}
