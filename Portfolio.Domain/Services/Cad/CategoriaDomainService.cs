using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Repositories.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Domain.Services.Cad
{
    public class CategoriaDomainService : CrudDomainServiceBase<ICategoriaRepository, Categoria>
    {
        private readonly AppSettings _appSettings;
        private ILogger<CategoriaDomainService> _logger;

        public CategoriaDomainService(IOptions<AppSettings> appSettings,
                                    ILogger<CategoriaDomainService> logger,
                                    ICategoriaRepository crudRepository) : base(crudRepository)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public List<Categoria> GetAll(string search)
        {
            return CrudRepository.GetAll(search);
        }
    }
}
