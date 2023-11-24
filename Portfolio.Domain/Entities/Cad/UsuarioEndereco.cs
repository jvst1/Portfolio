using Portfolio.Domain.Base;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Attributes;

namespace Portfolio.Domain.Entities.Cad
{
    public class UsuarioEndereco : EntityBase<long>
    {
        public Guid CodigoUsuario { get; set; }

        [Truncate(8)]
        public string CEP { get; set; }

        [Truncate(100)]
        public string Logradouro { get; set; }

        [Truncate(15)]
        public string NroLogradouro { get; set; }

        [Truncate(50)]
        public string Bairro { get; set; }

        [Truncate(50)]
        public string Complemento { get; set; }

        [Truncate(40)]
        public string Cidade { get; set; }

        [Truncate(2)]
        public string UF { get; set; }

        public DateTime? EnderecoDesde { get; set; }

        public bool? EnderecoPrincipal { get; set; }

        public DateTime? DtInclusao { get; set; }

        public DateTime? DtAlteracao { get; set; }

        protected override void InsertValidate()
        {
            if (CodigoUsuario.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule("O campo CodigoUsuario não pode ser nulo.", nameof(CodigoUsuario)));

            if (!EnderecoPrincipal.HasValue)
                EnderecoPrincipal = false;

            Codigo = Guid.NewGuid();
            DtInclusao = DateTime.Now;
        }

        protected override void UpdateValidate()
        {
            CEP = Util.DeixaNumeros(CEP);
            DtAlteracao = DateTime.Now;
        }
    }
}
