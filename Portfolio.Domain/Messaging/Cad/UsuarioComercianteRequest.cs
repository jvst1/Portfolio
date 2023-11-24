using Portfolio.Infrastructure.Enums;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioComercianteRequest : UsuarioRequest
    {
        public string TempoEntrega { get; set; }
        public string ValorMinimoPedido { get; set; }
        public string Tags { get; set; }
    }
}
