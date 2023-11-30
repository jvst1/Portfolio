using Portfolio.Infrastructure.Enums;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioRequest
    {
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
        public string DocumentoFederal { get; set; }
        public string Identificador { get; set; }
        public string Email { get; set; }
        public string TelefoneCelular { get; set; }
        public string ImageUrl { get; set; }
        public TipoPerfilUsuario? TipoPerfil { get; set; }
        public string ClientIdIdentityServer { get; set; }
    }
}
