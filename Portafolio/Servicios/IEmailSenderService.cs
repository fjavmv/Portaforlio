using Portafolio.Models;

namespace Portafolio.Servicios
{
    public interface IEmailSenderService
    {
        Task SenderEmailAsync(ContactoViewModel contacto);
    }
}
