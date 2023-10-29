using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Security;

namespace Portfolio.Application.Services
{
    public class AuthenticationApplicationService
    {
        private readonly UsuarioDomainService _usuarioDomainService;
        private readonly TokenManager _tokenService;

        public AuthenticationApplicationService(UsuarioDomainService usuarioService, TokenManager tokenService)
        {
            _usuarioDomainService = usuarioService;
            _tokenService = tokenService;
        }

        public LoginResponse Autenticar(LoginRequest request)
        {
            var user = _usuarioDomainService.GetFirstByEmail(request.Login);

            if (user == null || !Security.ValidatePassword(request.Senha, user.SenhaAcesso))
                throw new InvalidDataException("Usuário ou senha incorretos.");

            if (user.Situacao == SituacaoUsuario.Inativo)
                throw new InvalidDataException("Usuário não autorizado.");

            return GeraToken(user);
        }
        public async Task DesativarToken()
        {
            await _tokenService.DeactivateCurrentAsync();
        }
        public LoginResponse RefreshAccessToken(string token)
        {
            var usuario = RetornaUsuarioPeloRefreshToken(token);

            if (usuario.DtExpiracaoRefreshToken < DateTime.Now)
            {
                throw new Exception("Refresh token expirado.");
            }

            return GeraToken(usuario);
        }
        public void RevokeRefreshToken(string token)
        {
            _usuarioDomainService.RevokeRefreshToken(token);
        }

        private Usuario RetornaUsuarioPeloRefreshToken(string token)
        {
            var usuario = _usuarioDomainService.GetFirstByRefreshToken(token);

            if (usuario == null)
                throw new Exception("Refresh token inválido.");

            return usuario;
        }

        private LoginResponse GeraToken(Usuario usuario)
        {
            var webToken = _usuarioDomainService.GeraEAtualizaRefreshToken(usuario, out var refreshToken);

            return new LoginResponse
            {
                Email = usuario.Email,
                Identificador = usuario.Identificador,
                AccessToken = webToken.AccessToken,
                Expires = webToken.Expires,
                RefreshToken = refreshToken
            };
        }
    }
}
