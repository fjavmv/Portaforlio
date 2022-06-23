namespace Portafolio.Models
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }   

    }
}
