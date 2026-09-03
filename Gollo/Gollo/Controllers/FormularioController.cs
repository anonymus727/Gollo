using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Gollo.Models;

namespace Gollo.Controllers
{
    public class FormularioController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FormularioModel modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Por favor completa correctamente todos los campos requeridos.";
                return View(modelo);
            }

            try
            {
                var smtpUser = Environment.GetEnvironmentVariable("GOLLO_SMTP_USER");
                var smtpPassword = Environment.GetEnvironmentVariable("GOLLO_SMTP_PASSWORD");

                if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
                {
                    ViewBag.Error = "El servicio de correo no está configurado. Contacta al administrador.";
                    return View(modelo);
                }

                string cuerpo = $"Nombre: {modelo.Nombre}\n" +
                                $"Cédula: {modelo.Cedula}\n" +
                                $"Teléfono: {modelo.Telefono}\n" +
                                $"Categoría de producto: {modelo.CategoriaProducto}\n" +
                                $"Producto específico: {modelo.ProductoEspecifico}\n" +
                                $"Mensaje adicional: {modelo.Mensaje}";

                using var correo = new MailMessage();
                correo.From = new MailAddress(smtpUser);
                correo.To.Add(smtpUser);
                correo.Subject = "Nueva solicitud desde la web de Gollo Los Angeles";
                correo.Body = cuerpo;
                correo.IsBodyHtml = false;

                using var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                smtp.Send(correo);
                ViewBag.Exito = "Formulario enviado correctamente. Revisa tu correo.";
                ModelState.Clear();
                return View();
            }
            catch
            {
                ViewBag.Error = "No se pudo enviar el formulario. Verifica la configuración del correo e inténtalo nuevamente.";
                return View(modelo);
            }
        }
    }
}
