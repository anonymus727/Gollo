using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Gollo.Models;
using System.IO;
using System.Net.Mime;

namespace Gollo.Controllers
{
    public class FormularioController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnviarFormulario(FormularioModel modelo)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensajeError"] = "Por favor complete todos los campos requeridos correctamente.";
                return RedirectToAction("Contacto", "Home");
            }

            try
            {
                // Ruta plantilla
                string rutaPlantilla = Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "Templates", "GolloContactoTemplate.html");
                string htmlBody = System.IO.File.ReadAllText(rutaPlantilla);

                // Reemplazo de contenido
                htmlBody = htmlBody.Replace("{{NOMBRE}}", modelo.Nombre)
                                   .Replace("{{CORREO}}", modelo.Correo)
                                   .Replace("{{TELEFONO}}", modelo.Telefono)
                                   .Replace("{{PRODUCTO}}", modelo.ProductoInteres)
                                   .Replace("{{MENSAJE}}", modelo.Mensaje);

                string rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");

                // --- ENVÍO A EMPRESA ---
                MailMessage correoEmpresa = new MailMessage();
                correoEmpresa.From = new MailAddress("venomaucer@gmail.com", "Gollo Los Ángeles");
                correoEmpresa.To.Add("venomaucer@gmail.com");
                correoEmpresa.Subject = "Nuevo formulario desde Gollo Los Ángeles";
                correoEmpresa.IsBodyHtml = true;

                AlternateView vistaEmpresa = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                LinkedResource logoEmpresa = new LinkedResource(rutaLogo, MediaTypeNames.Image.Png)
                {
                    ContentId = "logoGollo",
                    TransferEncoding = TransferEncoding.Base64
                };
                vistaEmpresa.LinkedResources.Add(logoEmpresa);
                correoEmpresa.AlternateViews.Add(vistaEmpresa);

                // --- ENVÍO AL CLIENTE ---
                MailMessage correoCliente = new MailMessage();
                correoCliente.From = new MailAddress("venomaucer@gmail.com", "Gollo Los Ángeles");
                correoCliente.To.Add(modelo.Correo); // Cliente
                correoCliente.Subject = "Gracias por contactarnos - Gollo Los Ángeles";
                correoCliente.IsBodyHtml = true;

                // Personalización del mensaje para cliente
                string htmlCliente = htmlBody.Replace("Gracias por escribirnos.", "Gracias por contactarnos. Un asesor se comunicará pronto contigo.");
                AlternateView vistaCliente = AlternateView.CreateAlternateViewFromString(htmlCliente, null, MediaTypeNames.Text.Html);
                LinkedResource logoCliente = new LinkedResource(rutaLogo, MediaTypeNames.Image.Png)
                {
                    ContentId = "logoGollo",
                    TransferEncoding = TransferEncoding.Base64
                };
                vistaCliente.LinkedResources.Add(logoCliente);
                correoCliente.AlternateViews.Add(vistaCliente);

                // Configuración SMTP
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("venomaucer@gmail.com", "ekby qrjt yrfg gmbl"), // Contraseña de app
                    EnableSsl = true
                };

                // Envío
                smtp.Send(correoEmpresa);
                smtp.Send(correoCliente);

                TempData["MensajeExito"] = "¡Tu mensaje ha sido enviado correctamente!";
            }
            catch (System.Exception ex)
            {
                TempData["MensajeError"] = "Error al enviar el mensaje: " + ex.Message;
            }

            return RedirectToAction("Contacto", "Home");
        }
    }
}
