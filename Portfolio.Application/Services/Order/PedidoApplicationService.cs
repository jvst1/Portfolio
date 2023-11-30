using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Order;
using Portfolio.Domain.Messaging.Order;
using Portfolio.Domain.Services.Order;
using Portfolio.Infrastructure.Exceptions;

namespace Portfolio.Application.Services.Order
{
    public class PedidoApplicationService
    {
        private readonly PedidoDomainService _pedidoDomainService;
        private readonly IMappingService<Pedido> _mapper;

        public PedidoApplicationService(PedidoDomainService pedidoDomainService,
                                         IMappingService<Pedido> mapper)
        {
            _pedidoDomainService = pedidoDomainService;
            _mapper = mapper;
        }

        public PedidoResponse Post(PedidoPostRequest request, Guid codigoComerciante)
        {
            var pedidos = _pedidoDomainService.NovoPedido(request, codigoComerciante);

            return _mapper.ConvertFromDomain<PedidoResponse>(pedidos);
        }

        public PedidoResponse GetById(Guid id)
        {
            var pedido = _pedidoDomainService.GetById(id);
            if (pedido == null)
                throw new PortfolioException("Nenhum pedido encontrado para o codigo de pedido informado");
            
            return _mapper.ConvertFromDomain<PedidoResponse>(pedido);
        }
    }
}
