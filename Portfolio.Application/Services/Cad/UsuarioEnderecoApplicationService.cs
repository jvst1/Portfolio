using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Application.Services.Cad
{
    public class UsuarioEnderecoApplicationService
    {
        private readonly UsuarioEnderecoDomainService _usuarioEnderecoDomainService;
        private readonly IMappingService<UsuarioEndereco> _mapper;

        public UsuarioEnderecoApplicationService(UsuarioEnderecoDomainService usuarioEnderecoDomainService,
                                         IMappingService<UsuarioEndereco> mapper)
        {
            _usuarioEnderecoDomainService = usuarioEnderecoDomainService;
            _mapper = mapper;
        }

        public List<UsuarioEnderecoResponse> GetAll(Request request)
        {
            var search = request["search"]?.ToUpper();
            Guid.TryParse(request["CodigoUsuario"], out var codigoUsuario);
            bool? enderecoPrincipal = null;
            if (bool.TryParse(request["enderecoPrincipal"], out bool enderecoPrincipalParsed))
                enderecoPrincipal = enderecoPrincipalParsed;

            var enderecos = _usuarioEnderecoDomainService.GetAll(search, codigoUsuario, enderecoPrincipal);

            return _mapper.ConvertFromDomain<UsuarioEnderecoResponse>(enderecos);
        }

        public UsuarioEnderecoResponse GetById(Guid id)
        {
            var entity = _usuarioEnderecoDomainService.GetById(id);
            return _mapper.ConvertFromDomain<UsuarioEnderecoResponse>(entity);
        }

        public void Put(Guid id, UsuarioEnderecoPutRequest request)
        {
            var entity = _mapper.ConvertToDomain(request);
            _usuarioEnderecoDomainService.Update(id, entity);
        }

        public UsuarioEndereco Post(UsuarioEnderecoPostRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            _usuarioEnderecoDomainService.Insert(sender);
            return sender;
        }

        public void Delete(Guid id)
        {
            _usuarioEnderecoDomainService.Remove(id);
        }
    }
}
