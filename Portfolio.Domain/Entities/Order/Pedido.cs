using Portfolio.Domain.Base;
using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Domain.Entities.Order
{
    public class Pedido : EntityBase<long>
    {
        public Guid CodigoCliente { get; set; }
        public Guid CodigoClienteEndereco { get; set; }
        public Guid CodigoComerciante { get; set; }
        public decimal Valor { get; set; }
        public string FormaPagamento { get; set; }
        public DateTime? DtInclusao { get; set; }

        protected override void InsertValidate()
        {
            if (CodigoCliente.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O cliente do pedido deve ser informado", nameof(CodigoCliente)));
            if (CodigoClienteEndereco.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O endereço de entrega do cliente deve ser informado", nameof(CodigoClienteEndereco)));
            if (CodigoComerciante.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O comerciante do pedido deve ser informado", nameof(CodigoComerciante)));
            if (Valor <= 0)
                AddBrokenRule(new BusinessRule($"O Valor do pedido não pode ser zero ou menor que zero", nameof(Valor)));
            if (FormaPagamento.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule($"A forma de pagamento deve ser informada", nameof(FormaPagamento)));

            if (Codigo.Equals(Guid.Empty))
                Codigo = Guid.NewGuid();
            DtInclusao = DateTime.Now;
        }

        protected override void UpdateValidate()
        {
            if (CodigoCliente.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O cliente do pedido deve ser informado", nameof(CodigoCliente)));
            if (CodigoClienteEndereco.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O endereço de entrega do cliente deve ser informado", nameof(CodigoClienteEndereco)));
            if (CodigoComerciante.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule($"O comerciante do pedido deve ser informado", nameof(CodigoComerciante)));
            if (Valor <= 0)
                AddBrokenRule(new BusinessRule($"O Valor do pedido não pode ser zero ou menor que zero", nameof(Valor)));
            if (FormaPagamento.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule($"A forma de pagamento deve ser informada", nameof(FormaPagamento)));
        }
    }
}
