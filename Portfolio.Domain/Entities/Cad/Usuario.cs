using Newtonsoft.Json;
using Portfolio.Domain.Base;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Attributes;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Security;

namespace Portfolio.Domain.Entities.Cad
{
    public class Usuario : EntityBase<long>
    {
        [Truncate(30)]
        public string Identificador { get; set; }

        [Truncate(100)]
        public string Email { get; set; }

        [Truncate(255)]
        public string Nome { get; set; }

        [Truncate(14)]
        public string DocumentoFederal { get; set; }

        [Truncate(100)]
        public string TelefoneCelular { get; set; }

        [Truncate(100)]
        public string TempoEntrega { get; set; }

        public decimal ValorMinimoPedido { get; set; }

        [Truncate(100)]
        public string Tags { get; set; }

        public SituacaoUsuario? Situacao { get; set; }

        public TipoPerfilUsuario? TipoPerfil { get; set; }

        public DateTime? DtLiberacao { get; set; }

        public DateTime? DtBloqueio { get; set; }

        [AutomaticUpdateIgnore]
        [Truncate(100)]
        public string SenhaAcesso { get; set; }

        [Truncate(100)]
        public string SenhaAPI { get; set; }

        public DateTime? DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }

        [Truncate(100)]
        public string RefreshToken { get; set; }

        public DateTime? DtExpiracaoRefreshToken { get; set; }

        [Truncate(500)]
        public string TokenSenha { get; set; }

        public DateTime? DtValidadeTokenSenha { get; set; }

        [Truncate(400)]
        public string ClientIdIdentityServer { get; set; }

        public void SetSenha(string senha)
        {
            SenhaAcesso = Security.HashPassword(senha);
        }

        protected override void InsertValidate()
        {

            if (!Util.ValidaEmail(Email))
                AddBrokenRule(new BusinessRule("O endereço de e-mail fornecido é inválido. Por favor, insira um endereço de e-mail válido no formato usuario@dominio.com", nameof(Email)));

            if (!Tags.IsNullOrWhiteSpace())
            {
                if (Tags.Length > 100)
                {
                    var tagArray = JsonConvert.DeserializeObject<List<string>>(Tags);
                    tagArray.RemoveAt(tagArray.Count - 1);
                    Tags = JsonConvert.SerializeObject(tagArray);
                }
            }

            DocumentoFederal = Util.DeixaNumeros(DocumentoFederal);
            TelefoneCelular = Util.DeixaNumeros(TelefoneCelular);
            Situacao = SituacaoUsuario.Cadastrado;
            DtInclusao = DateTime.Now;
        }

        protected override void UpdateValidate()
        {
            DocumentoFederal = Util.DeixaNumeros(DocumentoFederal);
            TelefoneCelular = Util.DeixaNumeros(TelefoneCelular);
            DtAlteracao = DateTime.Now;
        }
    }
}
