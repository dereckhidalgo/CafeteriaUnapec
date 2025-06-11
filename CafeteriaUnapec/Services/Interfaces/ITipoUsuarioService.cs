using CafeteriaUnapec.DTOs.UsuariosDTO;

namespace CafeteriaUnapec.Services.Interfaces
{
    public interface ITipoUsuarioService
    {
        Task<IEnumerable<TipoUsuarioResponseDto>> GetAllTiposUsuariosAsync();
        Task<TipoUsuarioResponseDto?> GetTipoUsuarioByIdAsync(int id);
        Task<TipoUsuarioResponseDto> CreateTipoUsuarioAsync(TipoUsuarioCreateDto tipoUsuario);
        Task<bool> UpdateTipoUsuarioAsync(int id, TipoUsuarioUpdateDto tipoUsuario);
        Task<bool> DeleteTipoUsuarioAsync(int id);
    }
}
