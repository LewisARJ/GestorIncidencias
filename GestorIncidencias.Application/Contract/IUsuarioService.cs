using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;

namespace GestorIncidencias.Application.Contract;

public interface IUsuarioService
{
    ServiceResult<IEnumerable<UsuarioDto>> GetAllUsuarios();
    ServiceResult<UsuarioDto> GetUsuarioById(int id);
    ServiceResult<UsuarioDto> CreateUsuario(CreateUsuarioDto dto);
    ServiceResult<UsuarioDto> UpdateUsuario(int id, CreateUsuarioDto dto);
    ServiceResult<bool> DeleteUsuario(int id);
}
