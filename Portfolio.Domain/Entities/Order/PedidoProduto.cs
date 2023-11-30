using Portfolio.Domain.Base;

namespace Portfolio.Domain.Entities.Order
{
    public class PedidoProduto : EntityBase<long>
    {
        public Guid CodigoPedido { get; set; }
        public Guid CodigoProduto { get; set; }
        public int Quantidade { get; set; }
        public string Comentario { get; set; }
        public DateTime DtInclusao { get; set; }

        protected override void InsertValidate()
        {
            if (CodigoPedido.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O codigo do pedido deve ser informado", nameof(CodigoPedido)));
            if (CodigoProduto.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O codigo de produto deve ser informado", nameof(CodigoProduto)));
            if (Quantidade <= 0)
                AddBrokenRule(new BusinessRule($"A quantidade de produtos deve ser maior que zero para poder ser incluido em um pedido", nameof(Quantidade)));

            if (Codigo.Equals(Guid.Empty))
                Codigo = Guid.NewGuid();

            DtInclusao = DateTime.Now;
        }

        protected override void UpdateValidate()
        {
            if (CodigoPedido.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O codigo do pedido deve ser informado", nameof(CodigoPedido)));
            if (CodigoProduto.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O codigo de produto deve ser informado", nameof(CodigoProduto)));
            if (Quantidade <= 0)
                AddBrokenRule(new BusinessRule($"A quantidade de produtos deve ser maior que zero para poder ser incluido em um pedido", nameof(Quantidade)));
        }
    }
}
