using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Infrastructure.Enums;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioResponse : IRawQueryResponse
    {
        public Guid Codigo { get; set; }
        public string Identificador { get; set; }
        public string Email { get; set; }
        public string TelefoneCelular { get; set; }
        public SituacaoUsuario? Situacao { get; set; }
        public TipoPerfilUsuario? TipoPerfil { get; set; }
        public string ClientIdIdentityServer { get; set; }
    }
}
