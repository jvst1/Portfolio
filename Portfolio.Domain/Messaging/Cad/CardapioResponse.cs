using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.Messaging.Cad
{
    public class CardapioResponse : IRawQueryResponse
    {
        public Guid Codigo { get; set; }
        public Guid CodigoUsuario { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ImageUrl { get; set; }
        public decimal ValorProduto { get; set; }
        public decimal ValorDescontoFixo { get; set; }
        public decimal ValorDescontoPercentual { get; set; }
        public bool VendaAtiva { get; set; }
        public bool AplicarDesconto { get; set; }
        public DateTime? DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }
    }
}
