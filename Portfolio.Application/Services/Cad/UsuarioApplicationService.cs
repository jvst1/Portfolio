using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;
using Portfolio.Infrastructure.Security;

namespace Portfolio.Application.Services.Cad
{
    public class UsuarioApplicationService
    {
        private readonly UsuarioDomainService _usuarioService;
        private readonly TokenManager _tokenService;
        private readonly JwtHandler _jwtHandler;
        private readonly IMappingService<Usuario> _mapper;

        public UsuarioApplicationService(UsuarioDomainService usuarioService,
                                         TokenManager tokenService,
                                         JwtHandler jwtHandler,
                                         IMappingService<Usuario> mapper)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _jwtHandler = jwtHandler;
			_mapper = mapper;
        }
        public List<UsuarioResponse> GetAll(Request request)
        {
            var search = request["search"]?.ToUpper();
            var situacao = request["situacao", true];

            var usuarioes = _usuarioService.GetAll(search, situacao);

            return _mapper.ConvertFromDomain<UsuarioResponse>(usuarioes);
        }

        public List<GuidStringRawQueryResponse> GetToSelect()
        {
            return _usuarioService.GetToSelect();
        }

        public void Put(Guid id, UsuarioRequest request)
        {
            var entity = _mapper.ConvertToDomain(request);
            _usuarioService.Update(id, entity);
        }

        public Usuario Post(UsuarioRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            _usuarioService.Insert(sender);
            return sender;
        }

        public void Delete(Guid id)
        {
            _usuarioService.Remove(id);
        }

        public void TrocarSenha(UsuarioTrocarSenhaRequest request)
        {

            _usuarioService.TrocarSenha(_tokenService.CodigoUsuarioLogado, request.Login, request.Senha, request.NovaSenha);
        }

        public void RecuperarSenha(UsuarioRecuperarSenhaRequest request)
        {
            _usuarioService.RecuperarSenha(request.Token, request.CodigoUsuario, request.Login, request.NovaSenha);
        }

        public void ValidaDataExpiracaoToken(string token)
        {
            var validatedToken = _jwtHandler.ValidateTokenSenha(token);

            if (validatedToken.ValidTo.ToUniversalTime().Date < DateTime.UtcNow.Date)
                throw new Exception("Token expirado.");
        }

        public void SolicitarLinkSenha(UsuarioSolicitarLinkSenhaRequest request)
        {
            _usuarioService.SolicitarLinkSenha(request.Login, request.PrimeiroAcesso);
        }

        public void AtualizarClientIdIdentity(Guid codigoUsuario, string clientIdIdentityServer)
        {
            _usuarioService.AtualizarClientIdIdentity(codigoUsuario, clientIdIdentityServer);
        }

        public Usuario GetFirstByRefreshToken(string refreshToken)
        {
            return _usuarioService.GetFirstByRefreshToken(refreshToken);
        }

        public Usuario GetFirstByEmail(string email)
        {
            return _usuarioService.GetFirstByEmail(email);
        }

        public Usuario GetUsuarioById(Guid id)
        {
            return _usuarioService.GetById(id);
        }

        public UsuarioResponse GetById(Guid id)
        {
            var entity = _usuarioService.GetById(id);
            return _mapper.ConvertFromDomain<UsuarioResponse>(entity);
        }

        public Usuario GetFirstByClientId(string clientId)
        {
            return _usuarioService.GetFirstByClientId(clientId);
        }

        public UsuarioResponse GetFirstByIdentificador(string id)
        {
            var usuario = _usuarioService.GetFirstByIdentificador(id);
            return _mapper.ConvertFromDomain<UsuarioResponse>(usuario);
        }

    }
}
