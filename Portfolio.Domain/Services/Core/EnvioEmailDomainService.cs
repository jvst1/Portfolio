using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Entities.Core;
using Portfolio.Domain.Repositories.Core;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Portfolio.Domain.Services.Core
{
    public class EnvioEmailDomainService : CrudDomainServiceBase<IEnvioEmailRepository, EnvioEmail>
    {
        private const string EmailDe = "sistema@portfolio.com.br";
        private const string ReplyTo = "atendimento@portfolio.com.br";

        private readonly AppSettings _appSettings;
        private readonly IEnvioEmailRepository _emailRepository;
        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;
        private readonly ILogger<EnvioEmailDomainService> _logger;

        public EnvioEmailDomainService(IOptions<AppSettings> options,
                                       IEnvioEmailRepository emailRepository,
                                       IAmazonSimpleEmailService amazonSimpleEmailService,
                                       ILogger<EnvioEmailDomainService> logger) : base(emailRepository)
        {
            _appSettings = options.Value;
            _emailRepository = emailRepository;
            _amazonSimpleEmailService = amazonSimpleEmailService;
            _logger = logger;
        }

        public EnvioEmail RegistrarEmailRecuperarSenha(Usuario usuario, string token)
        {
            var email = GetEmail(usuario, new Dictionary<string, string>
            {
                {"{{TOKENSENHA}}", token},
                {"{{CODIGOUSUARIO}}", usuario.Codigo.ToString()},
                {"{{USUARIOEMAIL}}", usuario.Email}
            });

            email.Assunto = "Recuperação de senha";
            email.Texto = "<!DOCTYPE html><html lang=\"pt-br\"><head> <meta charset=\"utf-8\" /> <title>Recuperação de Senha</title> <style> body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; } .container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); } .header { text-align: center; background-color: #007bff; padding: 20px 0; } .header h1 { color: #fff; font-size: 24px; } .content { padding: 20px; } .content p { font-size: 16px; line-height: 1.5; margin-bottom: 20px; } .button { display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px; transition: background-color 0.3s; } .button:hover { background-color: #0056b3; } .footer { text-align: center; margin-top: 20px; color: #777; } </style></head><body><div class=\"container\"> <div class=\"header\"> <h1>Recuperação de Senha</h1> </div> <div class=\"content\"> <p>Prezado(a), {{NOME}},</p> <p>Para prosseguir com o primeiro acesso ou a recuperação de senha, clique no botão abaixo:</p> <a href=\"{{BASEURL}}/recuperar?c={{CODIGOUSUARIO}}&t={{TOKENSENHA}}&e={{USUARIOEMAIL}}\" class=\"button\">Recuperação de Senha</a> <p>Se o botão não funcionar, copie e cole o seguinte endereço em seu navegador:</p> <p>{{BASEURL}}/recuperar?c={{CODIGOUSUARIO}}&t={{TOKENSENHA}}&e={{USUARIO}}</p> </div> <div class=\"footer\"> Atenciosamente, <br> <a href=\"{{BASEURL}}\">Clique aqui para acessar nosso site</a> </div></div></body></html>";

            _emailRepository.Insert(email);
            _emailRepository.SaveChanges();

            return email;
        }

        public async Task<string> SendEmailAsync(EnvioEmail email)
        {
            try
            {
                if (!email.Replace.IsNullOrWhiteSpace())
                {
                    var variaveisParaReplace = JsonConvert.DeserializeObject<Dictionary<string, string>>(email.Replace);
                    foreach (var variable in variaveisParaReplace)
                        email.Texto = email.Texto.Replace(variable.Key, variable.Value);
                }

                var response = await _amazonSimpleEmailService.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination
                        {
                            BccAddresses = new List<string> { email.CopiaOculta ?? "" },
                            CcAddresses = new List<string> { email.Copia ?? "" },
                            ToAddresses = new List<string> { email.Para ?? "" }
                        },
                        Message = new Message
                        {
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = email.Texto
                                }
                            },
                            Subject = new Content
                            {
                                Charset = "UTF-8",
                                Data = email.Assunto
                            }
                        },
                        Source = email.De
                    });

                email.MessageId = response.MessageId;
                email.Enviado = true;
                email.DataEnvio = DateTime.Now;
                _emailRepository.Update(email);
            }
            catch (Exception ex)
            {
                _logger.LogError("ErrorMessage: {error}; Exception: {exception}", ex.Message, ex);

                email.Enviado = true;
                email.DataEnvio = DateTime.Now;
                email.Erro = $"{ex.Message};;; {ex}".Truncate(1000);

                _emailRepository.Update(email);
            }

            return email.MessageId;
        }

        private EnvioEmail GetEmail(Usuario usuario, Dictionary<string, string> parametros = null)
        {
            parametros.Add("BASEURL", _appSettings.WebUrl);
            parametros.Add("NOME", usuario.Identificador);

            var envioEmail = new EnvioEmail
            {
                Codigo = Guid.NewGuid(),
                DtInclusao = DateTime.Now,
                Para = usuario.Email,
                De = EmailDe,
                ReplyTo = ReplyTo,
            };

            if (parametros != null)
                envioEmail.Replace = JsonConvert.SerializeObject(parametros);

            return envioEmail;
        }
    }
}
