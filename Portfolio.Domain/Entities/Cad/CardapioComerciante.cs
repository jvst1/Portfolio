using Portfolio.Domain.Base;
using Portfolio.Infrastructure.Attributes;
using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Domain.Entities.Cad
{
    public class CardapioComerciante : EntityBase<long>
    {
        public Guid CodigoUsuario { get; set; }

        [Truncate(255)]
        public string Nome { get; set; }

        [Truncate(255)]
        public string Descricao { get; set; }

        public string ImageUrl { get; set; }

        public decimal ValorProduto { get; set; }

        public decimal ValorDescontoFixo { get; set; }

        public decimal ValorDescontoPercentual { get; set; }

        public bool VendaAtiva { get; set; }

        public bool AplicarDesconto { get; set; }

        public DateTime? DtInclusao { get; set; }

        public DateTime? DtAlteracao { get; set; }

        protected override void InsertValidate()
        {
            if (Nome.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule($"O nome do produto não pode ser nulo", nameof(Nome)));

            if (Descricao.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule($"A descrição do produto não pode ser nulo", nameof(Nome)));

            if (ImageUrl.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule($"O campo ImageUrl é obrigatório e deve ser informado", nameof(ImageUrl)));

            Codigo = Guid.NewGuid();
            DtInclusao = DateTime.Now;
        }

        protected override void UpdateValidate()
        {
            DtAlteracao = DateTime.Now;
        }
    }
}
