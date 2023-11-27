using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Api.Controllers.Cad
{
    public class CardapioComercianteController : PortfolioControllerBase
    {
        private readonly CardapioComercianteApplicationService _cardapioComercianteApplicationService;

        public CardapioComercianteController(CardapioComercianteApplicationService cardapioComercianteApplicationService)
        {
            _cardapioComercianteApplicationService = cardapioComercianteApplicationService;
        }

        [HttpGet]
        [Authorize()]
        public ActionResult<List<CardapioComercianteResponse>> GetAll(Request request)
        {
            return _cardapioComercianteApplicationService.GetAll(request);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult<CardapioComercianteResponse> Get(Guid id)
        {
            return _cardapioComercianteApplicationService.GetById(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public IActionResult Put(Guid id, CardapioComerciantePutRequest sender)
        {
            if (id != sender.Codigo)
                return BadRequest();

            _cardapioComercianteApplicationService.Put(id, sender);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult<UsuarioResponse> Post(CardapioComerciantePostRequest request)
        {
            var sender = _cardapioComercianteApplicationService.Post(request);
            return CreatedAtAction("Get", new { id = sender.Codigo }, sender);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult Delete(Guid id)
        {
            _cardapioComercianteApplicationService.Delete(id);
            return Ok();
        }
    }
}
