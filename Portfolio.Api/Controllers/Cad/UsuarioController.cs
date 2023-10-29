using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Controllers;
using Portfolio.Application.Services.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Safe.Api.Controllers.Cad
{
    [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
    public class UsuarioController : PortfolioControllerBase
    {
        private readonly UsuarioApplicationService _usuarioApplicationService;

        public UsuarioController(UsuarioApplicationService service)
        {
            _usuarioApplicationService = service;
        }

        [HttpGet("GetToSelect")]
        public ActionResult GetToSelect()
        {
            var response = _usuarioApplicationService.GetToSelect();
            return Ok(response);
        }

        [HttpGet]
        public ActionResult<List<UsuarioResponse>> Get(Request request)
        {
            return _usuarioApplicationService.GetAll(request);
        }

        [HttpGet("{id}")]
        public ActionResult<UsuarioResponse> Get(Guid id)
        {
            return _usuarioApplicationService.GetById(id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UsuarioRequest sender)
        {                                   
            if (id != sender.Codigo) 
                return BadRequest();

            _usuarioApplicationService.Put(id, sender);
            return Ok();
        }

        [HttpPost]
        public ActionResult<UsuarioResponse> Post(UsuarioRequest request)
        {
            var sender = _usuarioApplicationService.Post(request);
            return CreatedAtAction("Get", new { id = sender.Codigo }, sender);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _usuarioApplicationService.Delete(id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("RecuperarSenha")]
        public IActionResult RecuperarSenha([FromBody] UsuarioRecuperarSenhaRequest request)
        {
            _usuarioApplicationService.RecuperarSenha(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("SolicitarLinkSenha")]
        public IActionResult SolicitarLinkSenha([FromBody] UsuarioSolicitarLinkSenhaRequest request)
        {
            _usuarioApplicationService.SolicitarLinkSenha(request);
            return Ok();
        }

        [HttpPost("TrocarSenha")]
        public IActionResult TrocarSenha([FromBody] UsuarioTrocarSenhaRequest request)
        {
            _usuarioApplicationService.TrocarSenha(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("ValidaTokenSenha")]
        public IActionResult ValidaTokenSenha(string token)
        {
            _usuarioApplicationService.ValidaDataExpiracaoToken(token);
            return Ok();
        }
    }
}