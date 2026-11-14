using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;

namespace GestorIncidencias.Application.Contract;

public interface IEquipoService
{
    ServiceResult<IEnumerable<EquipoDto>> GetAllEquipos();
    ServiceResult<EquipoDto> GetEquipoById(int id);
    ServiceResult<EquipoDto> CreateEquipo(CreateEquipoDto dto);
    ServiceResult<EquipoDto> UpdateEquipo(int id, CreateEquipoDto dto);
    ServiceResult<bool> DeleteEquipo(int id);
}
