using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;
using System.Diagnostics;

namespace Portafolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioProyectos repositorioProyectos;
        private readonly IEmailSenderService emailSenderService;

        // private readonly IServicioEmail servicioEmail;

        public HomeController(
            IRepositorioProyectos repositorioProyectos, IEmailSenderService emailSenderService //IServicioEmail servicioEmail
            )
        {
            this.repositorioProyectos = repositorioProyectos;
            this.emailSenderService = emailSenderService;
            //this.servicioEmail = servicioEmail;
        }

        public IActionResult Index()
        {
            var proyectos = repositorioProyectos.ObtenerProyectos().Take(2).ToList();
          
            var modelo = new HomeIndexViewModel() { 
                Proyectos = proyectos
            };
            return View(modelo);
        }

        /**
         * Un Action o acción es un método en un Controller o controlador, 
         * que normalmente es accesible por una URL gracias al sistema de Ruteo o enrutamineto. 
         * Una accion o Action normalmente regresa un Action Result, basado en la interfaz IActionResult.
         */
        public IActionResult Proyectos()
        {
            //solo mostramos el listado de proyectos a la vista
            var proyectos = repositorioProyectos.ObtenerProyectos();
            return View(proyectos);
        }

        //Acción para acceder a la vista de contacto dentro del proyecto retornamos una vista
        [HttpGet]
        public IActionResult Contacto()
        {
            return View();
        }

        //podemos enviar información del controlador a l vista para enviar de la vista al controlador utilizaremos
        //un httpost
        //necesitams uan clase para colocar la data 
        //Vamoa  recibir los datos de conectato encapsulados en un objeto contatoviewmodel
        //atrubto para indicar que es uan acción httpost

        //utilizaremos un servicio https://sendgrid.com/ 
        //nos registramos
        //creater a single sender
        //llenamos formulario
        //recibir email
        //https://sendgrid.com/guide
        //start web api
        //web api
        //selccionamos el lenguaje
        //creamos nuestra llave lo guardamos
        
        [HttpPost]//atributo indico esta acción va funcionar cuando recbamos una acción
        public async Task<IActionResult> Contacto(ContactoViewModel contactoViewModel)
        {
            //permite que un método que ha llamado a otro método
            //asíncrono se espere a que dicho método asíncrono termine. 
            await emailSenderService.SenderEmailAsync(contactoViewModel);
            //retornamos una redirección en lugar de una vista 
            return RedirectToAction("Gracias");
        }

        //Esta accion sirve para una redirección luego de enviar el formulario
        //y no permitir al usuario enviar dos veces la misma información
        public IActionResult Gracias()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}