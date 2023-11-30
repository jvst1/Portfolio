using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.Messaging.Order
{
    public class PedidoResponse : IRawQueryResponse
    {
        public Guid Codigo { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; }
        public DateTime? DtInclusao { get; set; }
    }
}
