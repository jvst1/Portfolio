using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Repositories.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Domain.Services.Cad
{
    public class CardapioComercianteDomainService : CrudDomainServiceBase<ICardapioComercianteRepository, CardapioComerciante>
    {
        private readonly AppSettings _appSettings;
        private ILogger<CardapioComercianteDomainService> _logger;

        public CardapioComercianteDomainService(IOptions<AppSettings> appSettings,
                                    ILogger<CardapioComercianteDomainService> logger,
                                    ICardapioComercianteRepository usuarioEnderecoRepository) : base(usuarioEnderecoRepository)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public List<CardapioComerciante> GetAll(string search)
        {
            return CrudRepository.GetAll(search);
        }
    }
}
