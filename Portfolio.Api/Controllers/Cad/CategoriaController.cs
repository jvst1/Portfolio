using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Api.Controllers.Cad
{
    public class CategoriaController : PortfolioControllerBase
    {
        private readonly CategoriaApplicationService _categoriaApplicationService;

        public CategoriaController(CategoriaApplicationService categoriaApplicationService)
        {
            _categoriaApplicationService = categoriaApplicationService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Cliente) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult<List<CategoriaResponse>> GetAll(Request request)
        {
            return _categoriaApplicationService.GetAll(request);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador) + "," + nameof(TipoPerfilUsuario.Cliente) + "," + nameof(TipoPerfilUsuario.Comerciante))]
        public ActionResult<CategoriaResponse> GetById(Guid id)
        {
            return _categoriaApplicationService.GetById(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public IActionResult Put(Guid id, CategoriaPutRequest sender)
        {
            if (id != sender.Codigo)
                return BadRequest();

            _categoriaApplicationService.Put(id, sender);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<UsuarioResponse> Post(CategoriaPostRequest request)
        {
            var sender = _categoriaApplicationService.Post(request);
            return CreatedAtAction("Get", new { id = sender.Codigo }, sender);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult Delete(Guid id)
        {
            _categoriaApplicationService.Delete(id);
            return Ok();
        }
    }
}
