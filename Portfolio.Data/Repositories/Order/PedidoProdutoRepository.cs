using Portfolio.Data.Repositories.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Order;
using Portfolio.Domain.Repositories.Order;

namespace Portfolio.Data.Repositories.Order
{
    public class PedidoProdutoRepository : EntityFrameworkRepositoryBase<PedidoProduto>, IPedidoProdutoRepository
    {
        public PedidoProdutoRepository(IDbProvider dbProvider) : base(dbProvider)
        {
        }
    }
}
