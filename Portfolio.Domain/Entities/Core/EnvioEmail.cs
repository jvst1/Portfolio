using System;
using Portfolio.Domain.Base;
using Portfolio.Infrastructure.Attributes;
using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Domain.Entities.Core
{
    public class EnvioEmail : EntityBase<int>
    {
        [Truncate(255)]
        public string De { get; set; }

        public string Para { get; set; }

        public string Copia { get; set; }

        public string CopiaOculta { get; set; }

        public DateTime DtInclusao { get; set; }

        [Truncate(255)]
        public string Assunto { get; set; }

        public string Texto { get; set; } = string.Empty;

        public bool Enviado { get; set; }

        public DateTime? DataEnvio { get; set; }

        [Truncate(1000)]
        public string Erro { get; set; }

        [Truncate(100)]
        public string ReplyTo { get; set; }

        [Truncate(255)]
        public string MessageId { get; set; }
        public string Replace { get; set; }

        protected override void InsertValidate()
        {
            if (De.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule("O remetente deve ser informado ao tentar cadastrar um novo envio de email", nameof(De)));

            if (Para.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule("Um destinatário deve ser informado ao tentar cadastrar um novo envio de email", nameof(Para)));

            if (Assunto.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule("Um assunto deve ser informado ao tentar cadastrar um novo envio de email", nameof(Assunto)));

            if (Texto.IsNullOrWhiteSpace())
                AddBrokenRule(new BusinessRule("Um texto deve ser informado ao tentar cadastrar um novo envio de email", nameof(Texto)));
        }

        protected override void UpdateValidate()
        {
        }
    }
}
