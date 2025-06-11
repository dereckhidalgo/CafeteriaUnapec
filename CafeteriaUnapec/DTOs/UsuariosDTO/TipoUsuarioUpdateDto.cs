using System.ComponentModel.DataAnnotations;

namespace CafeteriaUnapec.DTOs.UsuariosDTO
{
    public class TipoUsuarioUpdateDto
    {
        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(100, ErrorMessage = "La descripción no puede exceder 100 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}
