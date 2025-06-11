using CafeteriaUnapec.Data;
using CafeteriaUnapec.DTOs.UsuariosDTO;
using CafeteriaUnapec.Model;
using CafeteriaUnapec.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CafeteriaUnapec.Services
{
    public class TipoUsuarioService: ITipoUsuarioService
    {
        private readonly CafeteriaDbContext _context;

        public TipoUsuarioService(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoUsuarioResponseDto>> GetAllTiposUsuariosAsync()
        {
            return await _context.TiposUsuarios
                .Where(x => x.Estado)
                 .Select(x => new TipoUsuarioResponseDto
                 {
                     Id = x.Id,
                     Descripcion = x.Descripcion,
                     Estado = x.Estado
                 })
                .ToListAsync();
        }

        public async Task<TipoUsuarioResponseDto?> GetTipoUsuarioByIdAsync(int id)
        {
            return await _context.TiposUsuarios
                .Where(x => x.Id == id && x.Estado)
                .Select(x => new TipoUsuarioResponseDto
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                })
                .FirstOrDefaultAsync(x => x.Id == id && x.Estado);
        }

        public async Task<TipoUsuarioResponseDto> CreateTipoUsuarioAsync(TipoUsuarioCreateDto createDto)
        {
            var tipoUsuario = new TipoUsuario
            {
                Descripcion = createDto.Descripcion,
                Estado = createDto.Estado
            };
            _context.TiposUsuarios.Add(tipoUsuario);
            await _context.SaveChangesAsync();
            return new TipoUsuarioResponseDto
            {
                Id = tipoUsuario.Id,
                Descripcion = tipoUsuario.Descripcion,
                Estado = tipoUsuario.Estado
            };
        }

        public async Task<bool> UpdateTipoUsuarioAsync(int id, TipoUsuarioUpdateDto input)
        {
            var tipoUsuario = await _context.TiposUsuarios.FindAsync(id);
            if (tipoUsuario is null) return false;

            tipoUsuario.Descripcion = input.Descripcion;
            tipoUsuario.Estado = input.Estado;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTipoUsuarioAsync(int id)
        {
            var tipoUsuario = await _context.TiposUsuarios.FindAsync(id);
            if (tipoUsuario is null) return false;

            tipoUsuario.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
