using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Order;
using Portfolio.Domain.Messaging.Order;

namespace Portfolio.Api.Controllers.Order
{
    public class PedidoController : PortfolioControllerBase
    {
        private readonly PedidoApplicationService _pedidoApplicationService;

        public PedidoController(PedidoApplicationService pedidoApplicationService)
        {
            _pedidoApplicationService = pedidoApplicationService;
        }

        [HttpPost("{codigoComerciante}")]
        [Authorize()]
        public ActionResult<PedidoResponse> Post(PedidoPostRequest request, Guid codigoComerciante)
        {
            var response = _pedidoApplicationService.Post(request, codigoComerciante);
            return CreatedAtAction("Get", new { id = response.Codigo }, response);
        }

        [HttpGet("{id}")]
        [Authorize()]
        public ActionResult<PedidoResponse> GetById(Guid id)
        {
            var response = _pedidoApplicationService.GetById(id);
            return CreatedAtAction("Get", new { id = response.Codigo }, response);
        }
    }
}
