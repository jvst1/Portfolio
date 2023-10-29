using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Cad;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using System.Security.Claims;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioControllerBase : ControllerBase
    {
        protected Usuario GetUsuarioLogado(UsuarioApplicationService usuarioService)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Usuario usuario;
            if (userId == null)
            {
                var clientId = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                usuario = usuarioService.GetFirstByClientId(clientId);
            }
            else
                usuario = usuarioService.GetUsuarioById(Guid.Parse(userId));

            if (usuario == null)
                throw new Exception("Usuario não permitido");

            return usuario;
        }

        protected ActionResult BadRequestResponse(string message)
        {
            var response = new Test
            {
                Sucesso = false,
                Mensagem = message
            };

            return BadRequest(response);
        }
    }

    public class Test : PortfolioResponseBase
    {
    }
}
