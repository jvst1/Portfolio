using Portfolio.Domain.Base;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioTrocarSenhaRequest : PorfolioRequestBase
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string NovaSenha { get; set; }
    }
}
