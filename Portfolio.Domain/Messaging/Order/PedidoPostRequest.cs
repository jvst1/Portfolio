namespace Portfolio.Domain.Messaging.Order
{
    public class PedidoPostRequest
    {
        public Guid CodigoCliente { get; set; }
        public Guid CodigoClienteEndereco { get; set; }
        public Guid CodigoComerciante { get; set; }
        public decimal Valor { get; set; }
        public string FormaPagamento { get; set; }
        public List<PedidoProdutoPostRequest> Produtos { get; set; }
    }
}
