using System.ComponentModel.DataAnnotations;

namespace Gollo.Models
{
    public class CedulaCostarricenseAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null) return false;
            var cedula = value.ToString()?.Trim().Replace("-", "").Replace(" ", "") ?? "";
            return cedula.Length == 9 && cedula.All(char.IsDigit);
        }
    }

    public class FormularioModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [CedulaCostarricense(ErrorMessage = "Ingrese una cédula válida de 9 dígitos.")]
        [Display(Name = "Cédula de identidad")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione una categoría.")]
        [Display(Name = "Categoría de producto")]
        public string CategoriaProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Especifique el producto que busca.")]
        [StringLength(200, ErrorMessage = "El producto no puede superar los 200 caracteres.")]
        [Display(Name = "¿Qué producto está buscando?")]
        public string ProductoEspecifico { get; set; } = string.Empty;

        [Display(Name = "Mensaje adicional")]
        public string? Mensaje { get; set; }
    }
}
