using Portfolio.Domain.Base;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioSolicitarLinkSenhaRequest : PorfolioRequestBase
    {
        public string Login { get; set; }

        public bool PrimeiroAcesso { get; set; }
    }
}
