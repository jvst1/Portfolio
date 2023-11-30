using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Repositories.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Domain.Services.Cad
{
    public class CardapioDomainService : CrudDomainServiceBase<ICardapioRepository, Cardapio>
    {
        private readonly AppSettings _appSettings;
        private ILogger<CardapioDomainService> _logger;

        public CardapioDomainService(IOptions<AppSettings> appSettings,
                                    ILogger<CardapioDomainService> logger,
                                    ICardapioRepository crudRepository) : base(crudRepository)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public List<Cardapio> GetAll(string search, Guid codigoComerciante)
        {
            return CrudRepository.GetAll(search, codigoComerciante);
        }

        public List<Cardapio> GetAllItensFromComerciante(Guid codigoComerciante)
        {
            return CrudRepository.GetAllItensFromComerciante(codigoComerciante);
        }

        public Cardapio GetItemByIdFromComerciante(Guid id, Guid codigoComerciante)
        {
            return CrudRepository.GetItemByIdFromComerciante(id, codigoComerciante);
        }

        public List<Cardapio> GetOfertas()
        {
            return CrudRepository.GetOfertas();
        }
    }
}
