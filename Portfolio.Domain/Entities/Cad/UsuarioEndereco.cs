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

            if (string.IsNullOrWhiteSpace(CEP))
                AddBrokenRule(new BusinessRule("CEP não pode ser nulo ou vazio.", nameof(CEP)));

            CEP = Util.DeixaNumeros(CEP);

            if (!string.IsNullOrWhiteSpace(CEP) && CEP.Length > 8)
                AddBrokenRule(new BusinessRule("O CEP fornecido é inválido. Confira os dados fornecidos. Tenha certeza que o CEP não ultrapasse 8 caracteres.", nameof(CEP)));

            if (!string.IsNullOrWhiteSpace(Logradouro) && Logradouro.Length > 100)
                AddBrokenRule(new BusinessRule("O Logradouro não pode ultrapassar 100 caracteres.", nameof(Logradouro)));

            if (!string.IsNullOrWhiteSpace(NroLogradouro) && NroLogradouro.Length > 15)
                AddBrokenRule(new BusinessRule("O Número do Logradouro não pode ultrapassar 15 caracteres.", nameof(NroLogradouro)));

            if (!string.IsNullOrWhiteSpace(Bairro) && Bairro.Length > 50)
                AddBrokenRule(new BusinessRule("O Bairro não pode ultrapassar 50 caracteres.", nameof(Bairro)));

            if (!string.IsNullOrWhiteSpace(Complemento) && Complemento.Length > 50)
                AddBrokenRule(new BusinessRule("O Complemento não pode ultrapassar 50 caracteres.", nameof(Complemento)));

            if (!string.IsNullOrWhiteSpace(Cidade) && Cidade.Length > 40)
                AddBrokenRule(new BusinessRule("A Cidade não pode ultrapassar 40 caracteres.", nameof(Cidade)));

            if (!string.IsNullOrWhiteSpace(UF) && UF.Length != 2)
                AddBrokenRule(new BusinessRule("UF deve ser preenchido com dois caracteres.", nameof(UF)));

            if (!EnderecoPrincipal.HasValue)
                EnderecoPrincipal = false;

            Codigo = Guid.NewGuid();
            DtInclusao = DateTime.Now;
        }


        protected override void UpdateValidate()
        {
            if (CodigoUsuario.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule("O campo CodigoUsuario não pode ser nulo.", nameof(CodigoUsuario)));

            if (string.IsNullOrWhiteSpace(CEP))
                AddBrokenRule(new BusinessRule("CEP não pode ser nulo ou vazio.", nameof(CEP)));

            CEP = Util.DeixaNumeros(CEP);

            if (!string.IsNullOrWhiteSpace(CEP) && CEP.Length > 8)
                AddBrokenRule(new BusinessRule("O CEP fornecido é inválido. Confira os dados fornecidos. Tenha certeza que o CEP não ultrapasse 8 caracteres.", nameof(CEP)));

            if (!string.IsNullOrWhiteSpace(Logradouro) && Logradouro.Length > 100)
                AddBrokenRule(new BusinessRule("O Logradouro não pode ultrapassar 100 caracteres.", nameof(Logradouro)));

            if (!string.IsNullOrWhiteSpace(NroLogradouro) && NroLogradouro.Length > 15)
                AddBrokenRule(new BusinessRule("O Número do Logradouro não pode ultrapassar 15 caracteres.", nameof(NroLogradouro)));

            if (!string.IsNullOrWhiteSpace(Bairro) && Bairro.Length > 50)
                AddBrokenRule(new BusinessRule("O Bairro não pode ultrapassar 50 caracteres.", nameof(Bairro)));

            if (!string.IsNullOrWhiteSpace(Complemento) && Complemento.Length > 50)
                AddBrokenRule(new BusinessRule("O Complemento não pode ultrapassar 50 caracteres.", nameof(Complemento)));

            if (!string.IsNullOrWhiteSpace(Cidade) && Cidade.Length > 40)
                AddBrokenRule(new BusinessRule("A Cidade não pode ultrapassar 40 caracteres.", nameof(Cidade)));

            if (!string.IsNullOrWhiteSpace(UF) && UF.Length != 2)
                AddBrokenRule(new BusinessRule("UF deve ser preenchido com dois caracteres.", nameof(UF)));

            if (!EnderecoPrincipal.HasValue)
                EnderecoPrincipal = false;

            DtAlteracao = DateTime.Now;
        }
    }
}
