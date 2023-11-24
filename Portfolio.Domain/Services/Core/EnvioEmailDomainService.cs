using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Entities.Core;
using Portfolio.Domain.Repositories.Core;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Domain.Services.Core
{
    public class EnvioEmailDomainService : CrudDomainServiceBase<IEnvioEmailRepository, EnvioEmail>
    {
        public EnvioEmailDomainService(IEnvioEmailRepository crudRepository) : base(crudRepository)
        {
        }

        private const string EmailDe = "sistema@portfolio.com.br";
        private const string ReplyTo = "atendimento@portfolio.com.br";

        private readonly IEnvioEmailRepository _emailRepository;
        private readonly AppSettings _appSettings;

        public EnvioEmailDomainService(IOptions<AppSettings> options,
            IEnvioEmailRepository emailRepository) : base(emailRepository)
        {
            _emailRepository = emailRepository;
            _appSettings = options.Value;
        }

        public EnvioEmail RegistrarEmailResetSenha(Usuario usuario, string token)
        {
            var email = GetEmail(usuario, new Dictionary<string, string>
            {
                {"TOKENSENHA", token},
                {"CODIGOUSUARIO", usuario.Codigo.ToString()},
                {"USUARIO", usuario.Email}
            });

            email.Assunto = "Recuperação de senha";
            email.Texto = ""; //todo: Montar corpo email

            _emailRepository.Insert(email);
            _emailRepository.SaveChanges();

            return email;
        }

        public void EnviarEmail(EnvioEmail email)
        {
            try
            {
                //todo: Criar envio de email com gmail

                //var response = _notificationServiceRequests.GetRequestSendNotification(JsonConvert.SerializeObject(request));

                //if (response.IsSuccessful)
                //    email.IdMensagem = JsonConvert.DeserializeObject<GuidIdRawQueryResponse>(response.Content).Id;
                //else
                //    email.Erro = $"Sucesso {response.IsSuccessful};;; Status code {response.StatusCode};;; Erro: {response.ErrorMessage ?? response.Content}";

                email.Enviado = true;
                email.DataEnvio = DateTime.Now;

                _emailRepository.Update(email);
            }
            catch (Exception e)
            {
                email.Enviado = true;
                email.DataEnvio = DateTime.Now;
                email.Erro = $"{e.Message};;; {e}".Truncate(1000);

                _emailRepository.Update(email);
            }
        }

        private EnvioEmail GetEmail(Usuario usuario, Dictionary<string, string> parametros = null)
        {
            var envioEmail = new EnvioEmail
            {
                Codigo = Guid.NewGuid(),
                DtInclusao = DateTime.Now,
                Para = usuario.Email,
                De = EmailDe,
                ReplyTo = ReplyTo
            };

            parametros.Add("BASEURL", _appSettings.WebUrl);
            parametros.Add("NOME", usuario.Identificador);

            return envioEmail;
        }
    }
}
