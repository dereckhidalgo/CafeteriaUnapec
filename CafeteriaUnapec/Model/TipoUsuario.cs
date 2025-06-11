using System.ComponentModel.DataAnnotations;

namespace CafeteriaUnapec.Model
{
    public class TipoUsuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;
    }
}
