using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;

namespace GestorIncidencias.Application.Contract;

public interface ITecnicoService
{
    ServiceResult<IEnumerable<TecnicoDto>> GetAllTecnicos();
    ServiceResult<TecnicoDto> GetTecnicoById(int id);
    ServiceResult<TecnicoDto> CreateTecnico(CreateTecnicoDto dto);
    ServiceResult<TecnicoDto> UpdateTecnico(int id, CreateTecnicoDto dto);
    ServiceResult<bool> DeleteTecnico(int id);
}
