using Portfolio.Domain.Base;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioRecuperarSenhaRequest : PorfolioRequestBase
    {
        public string Token { get; set; }

        public string Login { get; set; }

        public Guid CodigoUsuario { get; set; }

        public string NovaSenha { get; set; }
    }
}
