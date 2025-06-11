using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CafeteriaUnapec.Model
{
    public class Marca
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;

        // Navegación
        [JsonIgnore]
        public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}
