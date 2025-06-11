namespace CafeteriaUnapec.DTOs.UsuariosDTO
{
    public class TipoUsuarioResponseDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
