using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Order;
using Portfolio.Domain.Repositories.Order;

namespace Portfolio.Data.Repositories.Order
{
    public class PedidoRepository : EntityFrameworkRepositoryBase<Pedido>, IPedidoRepository
    {
        public PedidoRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }
    }
}
