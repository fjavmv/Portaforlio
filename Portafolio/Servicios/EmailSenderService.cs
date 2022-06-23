//using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
//using MimeKit;
using Portafolio.Models;
using System.Net;
using System.Net.Mail;
//using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace Portafolio.Servicios
{
    public class EmailSenderService : IEmailSenderService
    {
        //inyección de dependencias
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration configuration;
        public EmailSenderService(IConfiguration configuration,IOptions<SmtpSettings> smtpSettings)
        {
            this.configuration = configuration;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SenderEmailAsync(ContactoViewModel contacto)
        {
            _smtpSettings.Server = configuration.GetValue<string>("Server");
            _smtpSettings.Port = configuration.GetValue<int>("Port");
            _smtpSettings.ReceiverName = configuration.GetValue<string>("ReceiverName");
            _smtpSettings.ReceiverEmail = configuration.GetValue<string>("ReceiverEmail");
            _smtpSettings.UserName = configuration.GetValue<string>("UserName");
            _smtpSettings.Password = configuration.GetValue<string>("Password");

              //en este apartado configuramos la api key
              MailMessage mailMessage = new MailMessage();

              //mailMessage.From = new MailAddress(contacto.Email, contacto.Nombre);
              mailMessage.Sender = new MailAddress(contacto.Email, contacto.Nombre);
              mailMessage.To.Add(new MailAddress(_smtpSettings.ReceiverEmail,_smtpSettings.ReceiverName));
             // mailMessage.To.Add(new MailAddress(contacto.Nombre,contacto.Email));
              mailMessage.Subject = contacto.Asunto;
              mailMessage.Body = contacto.Mensaje;
              mailMessage.IsBodyHtml = true;

              SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server);
              smtpClient.EnableSsl = true;
              smtpClient.UseDefaultCredentials = false;
              //smtpClient.Host = "smtp.gmail.com";
              smtpClient.Port = _smtpSettings.Port;
              smtpClient.Credentials = new NetworkCredential(_smtpSettings.ReceiverEmail, _smtpSettings.Password);

              try
              {
                  smtpClient.Send(mailMessage);
              }
              catch (Exception ex)
              {
                  Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                      ex.ToString());
              }
              smtpClient.Dispose();

          /*  var message = new MimeMessage();
            message.From.Add(new MailboxAddress(contacto.Nombre, contacto.Email));
            message.To.Add(new MailboxAddress(_smtpSettings.ReceiverName, _smtpSettings.ReceiverEmail));
            message.Subject = contacto.Asunto;
            //texto plano html
            message.Body = new TextPart("text/plain") { Text = contacto.Mensaje };
            try
            {
                
              
                //Enviar el
                using (var cliente = new SmtpClient())
                {
                    await cliente.ConnectAsync(_smtpSettings.Server);
                    await cliente.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                    await cliente.SendAsync(message);
                    await cliente.DisconnectAsync(true);
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
                throw;
            }*/
        }
    }
}
