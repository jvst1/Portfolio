namespace Portfolio.Domain.Messaging.Order
{
    public class PedidoProdutoPostRequest
    {
        public Guid CodigoProduto { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string Comentario { get; set; }
    }
}
