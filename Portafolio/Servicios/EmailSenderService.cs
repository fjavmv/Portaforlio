using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Portafolio.Models;
using System.Linq;
using System.Net;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;
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
            //ene ste apartado configuramos la api key
            _smtpSettings.Server = configuration.GetValue<string>("Server");
            _smtpSettings.Port = configuration.GetValue<int>("Port");
            _smtpSettings.SenderName = configuration.GetValue<string>("SenderName");
            _smtpSettings.SenderEmail = configuration.GetValue<string>("SenderEmail");
            _smtpSettings.UserName = configuration.GetValue<string>("UserName");
            _smtpSettings.Password = configuration.GetValue<string>("Password");
           

            try
            {
                MailMessage mailMessage = new MailMessage(contacto.Email,_smtpSettings.SenderEmail, contacto.Nombre, contacto.Mensaje);
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = _smtpSettings.Port;
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.SenderEmail, _smtpSettings.Password);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();


               /* var message = new MimeMessage();
                message.From.Add(new MailboxAddress(contacto.Nombre,contacto.Email));
                message.To.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.Subject = contacto.Nombre;
                //texto plano html
                message.Body = new TextPart("text/plain") {Text = contacto.Mensaje };

                //Enviar el
                SmtpClient smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync(_smtpSettings.Server);
                
                await smtpClient.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
                /*using (var cliente = new SmtpClient())
                {
                    await cliente.ConnectAsync(_smtpSettings.Server);
                    await cliente.AuthenticateAsync(_smtpSettings.UserName,_smtpSettings.Password);
                    await cliente.SendAsync(message);
                    await cliente.DisconnectAsync(true);
                }*/
            }
            catch(Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
