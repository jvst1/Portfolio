using Portfolio.Domain.Entities.Core;

namespace Portfolio.Domain.Repositories.Core
{
	public interface IEmailAnexoRepository
	{
		List<EnvioEmailAnexo> GetAllByCodigoEmail(Guid emailCodigo);
	}
}