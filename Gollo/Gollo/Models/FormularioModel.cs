using System.ComponentModel.DataAnnotations;

namespace Gollo.Models
{
    public class FormularioModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string ProductoInteres { get; set; }

        [Required]
        public string Mensaje { get; set; }

        [Required]
        public string TieneCredigollo { get; set; } // Nuevo campo
    }
}
