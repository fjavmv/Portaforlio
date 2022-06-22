using Portafolio.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Portafolio.Servicios
{
    //servicio utilizando sendgrid
    //interface de sendgrid
    public interface IServicioEmail
    {
        Task Enviar(ContactoViewModel contacto);
    }

    public class ServicioEmailSendGrid : IServicioEmail
    {
        private readonly IConfiguration configuration;

        public ServicioEmailSendGrid(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Enviar(ContactoViewModel contacto)
        {
            //ene ste apartado configuramos la api key
            var apiKey = configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = configuration.GetValue<string>("SENDGRID_FROM");
            var nombre = configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subject = $"El cliente {    contacto.Email} quiere contactarte";
            var to = new EmailAddress(email, nombre);
            
            //variable de texto plano
            var mensajeTextoPlano = contacto.Mensaje;
            //varible con contenido html personalizable
            var contenidoHtml = @$"De: {contacto.Nombre} - Email: {contacto.Email} Mensaje: {contacto.Mensaje}";
            //Construimos el correo con mail helper
            var singleEmail = MailHelper.CreateSingleEmail(from, to, subject, mensajeTextoPlano, contenidoHtml);
            //enviamos el correo
            var respuesta = await cliente.SendEmailAsync(singleEmail);
        }
    }
}
