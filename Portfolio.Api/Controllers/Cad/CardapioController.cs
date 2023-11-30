using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Api.Controllers.Cad
{
    public class CardapioController : PortfolioControllerBase
    {
        private readonly CardapioApplicationService _cardapioApplicationService;

        public CardapioController(CardapioApplicationService cardapioApplicationService)
        {
            _cardapioApplicationService = cardapioApplicationService;
        }

        [HttpGet("{codigoComerciante}/Itens")]
        [Authorize()]
        public ActionResult<List<CardapioResponse>> GetAll(Request request, Guid codigoComerciante)
        {
            return _cardapioApplicationService.GetAll(request, codigoComerciante);
        }

        [HttpGet("{codigoComerciante}/Itens/{id}")]
        [Authorize()]
        public ActionResult<CardapioResponse> GetItemByIdFromComerciante(Guid id, Guid codigoComerciante)
        {
            return _cardapioApplicationService.GetItemByIdFromComerciante(id, codigoComerciante);
        }

        [HttpPut("{codigoComerciante}/Itens/{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public IActionResult Put(Guid id, CardapioPutRequest request, Guid codigoComerciante)
        {
            if (id != request.Codigo)
                return BadRequest();

            if (codigoComerciante != request.CodigoUsuario)
                return BadRequest();

            _cardapioApplicationService.Put(id, request);
            return Ok();
        }

        [HttpPost("{codigoComerciante}/Itens/")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult<UsuarioResponse> Post(CardapioPostRequest request, Guid codigoComerciante)
        {
            if (codigoComerciante != request.CodigoUsuario)
                return BadRequest();

            var sender = _cardapioApplicationService.Post(request);
            return CreatedAtAction("Get", new { id = sender.Codigo }, sender);
        }

        [HttpDelete("{codigoComerciante}/Itens/{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult Delete(Guid id, Guid codigoComerciante)
        {
            _cardapioApplicationService.Delete(id, codigoComerciante);
            return Ok();
        }

        [HttpGet("Oferta")]
        [Authorize()]
        public ActionResult<CardapioResponse> GetOferta()
        {
            var response = _cardapioApplicationService.GetOfertas();
            return Ok(response);
        }
    }
}
