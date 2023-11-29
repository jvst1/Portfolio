using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Api.Controllers.Cad
{
    [Authorize(Roles = nameof(TipoPerfilUsuario.Cliente))]
    public class UsuarioEnderecoController : PortfolioControllerBase
    {
        private readonly UsuarioEnderecoApplicationService _usuarioEnderecoApplicationService;

        public UsuarioEnderecoController(UsuarioEnderecoApplicationService usuarioEnderecoApplicationService)
        {
            _usuarioEnderecoApplicationService = usuarioEnderecoApplicationService;
        }

        [HttpGet]
        public ActionResult<List<UsuarioEnderecoResponse>> GetAll(Request request)
        {
            return _usuarioEnderecoApplicationService.GetAll(request);
        }

        [HttpGet("{id}")]
        public ActionResult<UsuarioEnderecoResponse> GetById(Guid id)
        {
            return _usuarioEnderecoApplicationService.GetById(id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UsuarioEnderecoPutRequest sender)
        {
            if (id != sender.Codigo)
                return BadRequest();

            _usuarioEnderecoApplicationService.Put(id, sender);
            return Ok();
        }

        [HttpPost]
        public ActionResult<UsuarioResponse> Post(UsuarioEnderecoPostRequest request)
        {
            var sender = _usuarioEnderecoApplicationService.Post(request);
            return CreatedAtAction("Get", new { id = sender.Codigo }, sender);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _usuarioEnderecoApplicationService.Delete(id);
            return Ok();
        }
    }
}
