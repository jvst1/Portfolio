using Portfolio.Domain.Base;
using Portfolio.Infrastructure.Attributes;
using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Domain.Entities.Cad
{
    public class Categoria : EntityBase<long>
    {
        [Truncate(255)]
        public string Nome { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? DtInclusao { get; set; }

        public DateTime? DtAlteracao { get; set; }

        protected override void InsertValidate()
        {
            if (Nome.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule($"O nome da categoria não pode ser nulo", nameof(Nome)));

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
