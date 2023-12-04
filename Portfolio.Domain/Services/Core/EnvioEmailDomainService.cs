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
using Portfolio.Infrastructure;
using System.Net.Mail;
using System.Net;

namespace Portfolio.Domain.Services.Core
{
    public class EnvioEmailDomainService : CrudDomainServiceBase<IEnvioEmailRepository, EnvioEmail>
    {
        private const string EmailDe = "jvst1portfolio@gmail.com";
        private const string ReplyTo = "jvst1portfolio@gmail.com";

        private readonly AppSettings _appSettings;
        private readonly IEnvioEmailRepository _emailRepository;
        private readonly ILogger<EnvioEmailDomainService> _logger;

        public EnvioEmailDomainService(IOptions<AppSettings> options,
                                       IEnvioEmailRepository emailRepository,
                                       ILogger<EnvioEmailDomainService> logger) : base(emailRepository)
        {
            _appSettings = options.Value;
            _emailRepository = emailRepository;
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
            email.Texto = "<!DOCTYPE html><html lang=\"pt-br\"><head> <meta charset=\"utf-8\" /> <title>Recuperação de Senha</title> <style> body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; } .container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); } .header { text-align: center; background-color: #007bff; padding: 20px 0; } .header h1 { color: #fff; font-size: 24px; } .content { padding: 20px; } .content p { font-size: 16px; line-height: 1.5; margin-bottom: 20px; } .button { display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px; transition: background-color 0.3s; } .button:hover { background-color: #0056b3; } .footer { text-align: center; margin-top: 20px; color: #777; } </style></head><body><div class=\"container\"> <div class=\"header\"> <h1>Recuperação de Senha</h1> </div> <div class=\"content\"> <p>Prezado(a), {{NOME}},</p> <p>Para prosseguir com o primeiro acesso ou a recuperação de senha, clique no botão abaixo:</p> <a href=\"{{BASEURL}}/recuperar?c={{CODIGOUSUARIO}}&t={{TOKENSENHA}}&e={{USUARIOEMAIL}}\" class=\"button\">Recuperação de Senha</a> <p>Se o botão não funcionar, copie e cole o seguinte endereço em seu navegador:</p> <p>{{BASEURL}}/recuperar?c={{CODIGOUSUARIO}}&t={{TOKENSENHA}}&e={{USUARIOEMAIL}}</p> </div> <div class=\"footer\"> Atenciosamente, <br> <a href=\"{{BASEURL}}\">Clique aqui para acessar nosso site</a> </div></div></body></html>";

            _emailRepository.Insert(email);
            _emailRepository.SaveChanges();

            return email;
        }

        public EnvioEmail RegistrarEmailNovoPedido(Usuario comerciante, string tabela, long numeroPedido, decimal valorPedido)
        {
            var email = GetEmail(comerciante, new Dictionary<string, string>
            {
                {"{{NUMEROPEDIDO}}", numeroPedido.ToString()},
                {"{{LINHASTABELA}}", tabela},
                {"{{VALORTOTALPEDIDO}}", Util.FormatarDecimal(valorPedido)}
            });

            email.Assunto = "Novo pedido!";
            email.Texto = "<!DOCTYPE html><html lang=\"pt-br\"><head> <meta charset=\"utf-8\" /> <title>Novo Pedido</title> <style> body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; } .container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); } .header { text-align: center; background-color: #28a745; padding: 20px 0; } .header h1 { color: #fff; font-size: 24px; } .content { padding: 20px; } .content p { font-size: 16px; line-height: 1.5; margin-bottom: 20px; } .button { display: inline-block; padding: 10px 20px; background-color: #28a745; color: #fff; text-decoration: none; border-radius: 5px; transition: background-color 0.3s; } .button:hover { background-color: #218838; } .footer { text-align: center; margin-top: 20px; color: #777; } </style></head><body> <div class=\"container\"> <div class=\"header\"> <h1>Novo Pedido Recebido! #{{NUMEROPEDIDO}}</h1> </div> <div class=\"content\"> <p>Prezado(a) {{NOME}},</p> <p>Você recebeu um novo pedido! Confira abaixo os detalhes do pedido:</p> <table border=\"1\" style=\"width:100%; border-collapse: collapse;\"> <thead> <tr> <th>Nome Produto</th> <th>Quantidade</th> <th>Preço</th> <th>Comentário Especial</th> <th>Endereço de Entrega</th> </tr> </thead> <tbody> {{LINHASTABELA}} </tbody> <tfoot> <tr> <td colspan=\"4\" style=\"text-align: right;\">Valor Total:</td> <td>{{VALORTOTALPEDIDO}}</td> </tr> </tfoot> </table></div> <p>É importante que o pedido seja processado o mais rápido possível para garantir a satisfação do cliente.</p> </div> <div class=\"footer\"> Atenciosamente, <br> <a href=\"{{BASEURL}}\">Clique aqui para acessar nosso site</a> </div> </div></body></html>";

            _emailRepository.Insert(email);
            _emailRepository.SaveChanges();

            return email;
        }

        public async Task<string> SendGmailAsync(EnvioEmail email)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(EmailDe);
                    message.Subject = email.Assunto;
                    message.To.Add(new MailAddress(email.Para));

                    if (!string.IsNullOrWhiteSpace(email.Copia))
                        message.CC.Add(new MailAddress(email.Copia));

                    if (!string.IsNullOrWhiteSpace(email.CopiaOculta))
                        message.Bcc.Add(new MailAddress(email.CopiaOculta));

                    message.IsBodyHtml = true;
                    if (!email.Replace.IsNullOrWhiteSpace())
                    {
                        var variaveisParaReplace = JsonConvert.DeserializeObject<Dictionary<string, string>>(email.Replace);
                        foreach (var variable in variaveisParaReplace)
                            email.Texto = email.Texto.Replace(variable.Key, variable.Value);
                    }
                    else
                        message.Body = email.Texto;

                    using (var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(_appSettings.Gmail.AccessKeyId, _appSettings.Gmail.SecretAccessKey),
                        EnableSsl = true
                    })
                    {
                        await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                    }

                    email.MessageId = $"{DateTime.Now:yyyyMMddhhmmssFFFFF}{DateTimeOffset.Now.ToUnixTimeMilliseconds()}".Substring(5, 7);
                    email.Enviado = true;
                    email.DataEnvio = DateTime.Now;
                    _emailRepository.Update(email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ErrorMessage: {error}; Exception: {exception}", ex.Message, ex);
                email.MessageId = null;
                email.Enviado = false;
                email.DataEnvio = DateTime.Now;
                email.Erro = $"{ex.Message};;; {ex}".Truncate(1000);
                _emailRepository.Update(email);
            }

            return email.MessageId;
        }


        public async Task<string> SendSimpleEmailServiceAsync(EnvioEmail email)
        {
            try
            {
                if (!email.Replace.IsNullOrWhiteSpace())
                {
                    var variaveisParaReplace = JsonConvert.DeserializeObject<Dictionary<string, string>>(email.Replace);
                    foreach (var variable in variaveisParaReplace)
                        email.Texto = email.Texto.Replace(variable.Key, variable.Value);
                }

                var response = await new AmazonSimpleEmailServiceClient(_appSettings.Ses.AccessKeyId, _appSettings.Ses.SecretAccessKey, Amazon.RegionEndpoint.GetBySystemName(_appSettings.Ses.RegionEndpoint)).SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination
                        {
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
            parametros.Add("{{BASEURL}}", _appSettings.WebUrl);
            parametros.Add("{{NOME}}", usuario.Identificador);

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
