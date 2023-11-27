using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Repositories.Cad;
using Portfolio.Domain.Services.Core;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Exceptions;
using Portfolio.Infrastructure.Helpers;
using Portfolio.Infrastructure.Security;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace Portfolio.Domain.Services.Cad
{
    public class UsuarioDomainService : CrudDomainServiceBase<IUsuarioRepository, Usuario>
    {
        private readonly AppSettings _appSettings;
        private ILogger<UsuarioDomainService> _logger;
        private readonly JwtHandler _jwtHandler;
        private readonly CodigoUsuarioHelper _codigoUsuarioHelper;
        private readonly EnvioEmailDomainService _emailService;

        public UsuarioDomainService(IOptions<AppSettings> appSettings,
                                    ILogger<UsuarioDomainService> logger,
                                    IUsuarioRepository crudRepository,
                                    JwtHandler jwtHandler,
                                    CodigoUsuarioHelper codigoUsuarioHelper,
                                    EnvioEmailDomainService emailService) : base(crudRepository)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _jwtHandler = jwtHandler;
            _emailService = emailService;
            _codigoUsuarioHelper = codigoUsuarioHelper;
        }

        public List<Usuario> GetAll(string search, string[] situacao)
        {
            return CrudRepository.GetAll(search, situacao);
        }
        
        public List<Usuario> GetAllComerciantes()
        {
            return CrudRepository.GetAllComerciantes();
        }

        public List<GuidStringRawQueryResponse> GetToSelect()
        {
            return CrudRepository.GetToSelect();
        }

        public override void Insert(Usuario entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "O objeto 'Usuário' não pode ser nulo ao tentar inserir um novo usuário.");

            if (string.IsNullOrWhiteSpace(entity.Email))
                throw new ArgumentException("O campo 'Email' do usuário é obrigatório e não pode estar vazio ou conter apenas espaços em branco.");

            if (GetFirstByEmail(entity.Email) != null)
                throw new ValidationException($"Já existe um usuário cadastrado com o e-mail '{entity.Email}'. Utilize outro e-mail para o cadastro.");

            base.Insert(entity);

            try
            {
                SolicitarLinkSenha(entity.Email, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Falha ao enviar o link de senha para o usuário.", ex);
            }
        }

        public Usuario GetFirstByEmail(string email)
        {
            return CrudRepository.GetFirstByEmail(email);
        }

        public void SolicitarLinkSenha(string requestLogin, bool requestPrimeiroAcesso)
        {
            if (string.IsNullOrWhiteSpace(requestLogin))
                throw new ArgumentException("O login fornecido é inválido.");

            var usuario = GetFirstByEmail(requestLogin);
            if (usuario == null)
                throw new UserNotFoundException("Usuário não encontrado.");

            if (usuario.Situacao == SituacaoUsuario.Inativo)
                throw new UserInactiveException("O usuário está inativo e não pode solicitar redefinição de senha.");

            if (usuario.SenhaAcesso != null && requestPrimeiroAcesso)
                throw new UserAlreadyActivatedException("O cadastro já foi ativado. Se você esqueceu sua senha, use a opção de redefinição de senha.");

            var token = _jwtHandler.GeraTokenSenha(usuario.Codigo, requestPrimeiroAcesso);
            var expiryMinutesSenha = _appSettings.ExpiryMinutesSenha;
            var dtExpiracao = DateTime.UtcNow.AddMinutes(expiryMinutesSenha);
            usuario.TokenSenha = token;
            usuario.DtValidadeTokenSenha = dtExpiracao;

            CrudRepository.Update(usuario);

            _emailService.RegistrarEmailResetSenha(usuario, token);
        }

        public void RecuperarSenha(string requestToken, Guid requestCodigoUsuario, string requestLogin, string requestNovaSenha)
        {
            var validatedToken = _jwtHandler.ValidateTokenSenha(requestToken);

            if (validatedToken == null)
                throw new InvalidDataException("O token fornecido é inválido ou não pôde ser validado.");

            if (!Guid.TryParse(validatedToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value, out var codigoUsuario))
                throw new InvalidOperationException("Não foi possível obter o código do usuário a partir do token.");

            var primeiraVez = validatedToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value == "first";
            var usuario = CrudRepository.GetById(codigoUsuario, true);

            if (usuario == null)
                throw new UserNotFoundException("Usuário não encontrado na base de dados.");

            if (usuario.TokenSenha != requestToken ||
                usuario.Codigo != requestCodigoUsuario ||
                usuario.Email != requestLogin)
                throw new InvalidDataException("As informações fornecidas não correspondem às registradas no sistema.");

            if (usuario.DtValidadeTokenSenha?.ToUniversalTime().Date != validatedToken.ValidTo.ToUniversalTime().Date ||
                validatedToken.ValidTo.ToUniversalTime().Date < DateTime.UtcNow.Date)
                throw new InvalidDataException("O token fornecido está expirado.");

            if (usuario.SenhaAcesso != null && primeiraVez)
                throw new UserAlreadyActivatedException("O cadastro deste usuário já foi ativado anteriormente.");

            usuario.SetSenha(requestNovaSenha);
            usuario.TokenSenha = null;
            usuario.DtValidadeTokenSenha = null;

            if (primeiraVez)
                usuario.Situacao = SituacaoUsuario.Ativo;

            CrudRepository.Update(usuario);
        }

        public void TrocarSenha(Guid codigoUsuarioLogado, string requestLogin, string requestSenha, string requestNovaSenha)
        {
            if (string.IsNullOrWhiteSpace(requestLogin) || string.IsNullOrWhiteSpace(requestSenha) || string.IsNullOrWhiteSpace(requestNovaSenha))
                throw new ArgumentException("O login, senha e nova senha são obrigatórios e não podem estar vazios.");

            var usuario = GetFirstByEmail(requestLogin);
            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            if (codigoUsuarioLogado != usuario.Codigo)
                throw new AuthenticationException("O usuário logado atualmente não tem permissão para alterar a senha deste usuário.");

            if (!Security.ValidatePassword(requestSenha, usuario.SenhaAcesso))
                throw new Exception("Usuário ou Senha Incorretos.");

            usuario.SetSenha(requestNovaSenha);
            CrudRepository.Update(usuario);
        }

        public void AtualizarClientIdIdentity(Guid codigoUsuario, string clientIdIdentityServer)
        {
            if (string.IsNullOrEmpty(clientIdIdentityServer))
                throw new ArgumentException("O 'ClientId' não pode ser nulo ou vazio.", nameof(clientIdIdentityServer));

            if (CrudRepository.IsClientIdCadastrado(codigoUsuario, clientIdIdentityServer))
                throw new InvalidOperationException("O 'ClientId' fornecido já está associado a outro usuario. Por favor, forneça um 'ClientId' único.");

            var usuarioAtual = CrudRepository.GetFirstByClientId(clientIdIdentityServer);
            if (usuarioAtual != null)
            {
                usuarioAtual.ClientIdIdentityServer = null;
                CrudRepository.Update(usuarioAtual);
            }

            var usuarioNovo = GetById(codigoUsuario);
            if (usuarioNovo == null)
                throw new Exception($"Não foi possível encontrar um usuario com o código {codigoUsuario} para atualizar o 'ClientId'.");

            usuarioNovo.ClientIdIdentityServer = clientIdIdentityServer;
            CrudRepository.Update(usuarioNovo);
        }

        public Usuario GetFirstByClientId(string clientId)
        {
            return CrudRepository.GetFirstByClientId(clientId);
        }

        public Usuario GetFirstByIdentificador(string id)
        {
            return CrudRepository.GetFirstByIdentificador(id);
        }

        public Usuario GetFirstByRefreshToken(string refreshToken)
        {
            return CrudRepository.GetFirstByRefreshToken(refreshToken);
        }

        public WebToken GeraEAtualizaRefreshToken(Usuario usuario, out string refreshToken)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Usuário não pode ser nulo.");

            var webToken = _jwtHandler.GeraTokenAutenticacao(usuario.Codigo, usuario.Email, usuario.TipoPerfil.GetValueOrDefault());
            refreshToken = GeraRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.DtExpiracaoRefreshToken = DateTime.UtcNow.AddMinutes(webToken.Expires + 5);

            _codigoUsuarioHelper.CodigoUsuario = usuario.Codigo;

            try
            {
                CrudRepository.Update(usuario);
                CrudRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Falha ao atualizar as informações do usuário no banco de dados.", ex);
            }
            return webToken;
        }

        private string GeraRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public void RevokeRefreshToken(string token)
        {
            var usuario = GetFirstByRefreshToken(token);
            usuario.RefreshToken = null;

            _codigoUsuarioHelper.CodigoUsuario = usuario.Codigo;

            CrudRepository.Update(usuario);
            CrudRepository.SaveChanges();
        }
    }
}
