using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services;
using Portfolio.Domain.Messaging.Cad;

namespace Portfolio.Api.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : PortfolioControllerBase
    {
        private readonly AuthenticationApplicationService _authenticationApplicationService;
        public AuthenticationController(AuthenticationApplicationService authenticationApplicationService)
        {
            _authenticationApplicationService = authenticationApplicationService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            var result = _authenticationApplicationService.Autenticar(request);
            return Ok(result);
        }

        [HttpDelete("Logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            await _authenticationApplicationService.DesativarToken();
            return NoContent();
        }

        [HttpPost("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        public ActionResult<LoginResponse> RefreshToken(UsuarioRefreshTokenRequest dto)
        {
            var result = _authenticationApplicationService.RefreshAccessToken(dto.RefreshToken);
            return Ok(result);
        }

        [HttpDelete("RefreshToken/{token}/Revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult RevokeRefreshToken(string token)
        {
            _authenticationApplicationService.RevokeRefreshToken(token);
            return NoContent();
        }
    }
}

