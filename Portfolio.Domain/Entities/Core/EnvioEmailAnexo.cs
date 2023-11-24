using System;
using Portfolio.Domain.Base;
using Portfolio.Infrastructure.Attributes;

namespace Portfolio.Domain.Entities.Core
{
    public class EnvioEmailAnexo : EntityBase<int>
    {
        public Guid CodigoEnvioEmail { get; set; }

        public byte[] Arquivo { get; set; }

        [Truncate(255)]
        public string NomeArquivo { get; set; }

        [Truncate(20)]
        public string Extensao { get; set; }

        protected override void InsertValidate()
        {
            if (CodigoEnvioEmail.Equals(Guid.Empty))
                AddBrokenRule(new BusinessRule("Deve ser informado qual o email o anexo pertence ao tentar cadastrar um novo anexo email", nameof(CodigoEnvioEmail)));

            if (Arquivo.Equals(default(byte[])))
                AddBrokenRule(new BusinessRule("O arquivo a ser enviado como anexo deve ser informado", nameof(Arquivo)));
        }

        protected override void UpdateValidate()
        {
        }
    }
}