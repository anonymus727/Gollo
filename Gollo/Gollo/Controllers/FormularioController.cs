using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Gollo.Models;

namespace Gollo.Controllers
{
    public class FormularioController : Controller
    {
        // GET: /Formulario/
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Formulario/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FormularioModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string cuerpo = $"Nombre: {modelo.Nombre}\n" +
                                    $"Correo: {modelo.Correo}\n" +
                                    $"Teléfono: {modelo.Telefono}\n" +
                                    $"Producto de interés: {modelo.ProductoInteres}\n" +
                                    $"Mensaje adicional: {modelo.Mensaje}";

                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress("venomaucer@gmail.com");
                    correo.To.Add("venomaucer@gmail.com");
                    correo.Subject = "Nuevo formulario enviado desde la web";
                    correo.Body = cuerpo;
                    correo.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("venomaucer@gmail.com", "ekby qrjt yrfg gmbl");
                    smtp.EnableSsl = true;

                    smtp.Send(correo);

                    ViewBag.Exito = "Formulario enviado correctamente. Revisa tu correo.";
                    ModelState.Clear(); // Limpia el formulario
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al enviar el formulario: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Error = "Por favor completa todos los campos requeridos.";
            }

            return View();
        }
    }
}
