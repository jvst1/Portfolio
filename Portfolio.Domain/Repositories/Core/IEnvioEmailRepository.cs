using Portfolio.Domain.Base.Interfaces.Repositories;
using Portfolio.Domain.Entities.Core;

namespace Portfolio.Domain.Repositories.Core
{
    public interface IEnvioEmailRepository : ICrudRepository<EnvioEmail>
    {
        List<EnvioEmail> GetAll(DateTime dtCorte, bool enviado, int limite);
    }
}
