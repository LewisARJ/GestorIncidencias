using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;

namespace GestorIncidencias.Application.Contract;

public interface IIncidenciaService
{
    ServiceResult<IEnumerable<IncidenciaDto>> GetAllIncidencias(string? estado);
    ServiceResult<IncidenciaDto> GetIncidenciaById(int id);
    ServiceResult<IncidenciaDto> CreateIncidencia(CreateIncidenciaDto dto);
    ServiceResult<IncidenciaDto> UpdateIncidencia(int id, UpdateIncidenciaDto dto);
    ServiceResult<bool> DeleteIncidencia(int id);
}
