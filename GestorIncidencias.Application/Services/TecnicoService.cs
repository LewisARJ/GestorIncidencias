using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;
using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;

namespace GestorIncidencias.Application.Services;

public class TecnicoService : ITecnicoService
{
    private readonly IBaseRepository<Tecnico> _tecnicos;
    private readonly IIncidenciaRepository _incidencias;

    public TecnicoService(IBaseRepository<Tecnico> tecnicos, IIncidenciaRepository incidencias)
    {
        _tecnicos = tecnicos;
        _incidencias = incidencias;
    }

    public ServiceResult<IEnumerable<TecnicoDto>> GetAllTecnicos()
    {
        var lista = _tecnicos.GetAll().Select(ToDto).ToList();
        return ServiceResult<IEnumerable<TecnicoDto>>.SuccessResponse(lista);
    }

    public ServiceResult<TecnicoDto> GetTecnicoById(int id)
    {
        var tecnico = _tecnicos.GetById(id);
        if (tecnico == null)
            return ServiceResult<TecnicoDto>.ErrorResponse("El técnico no existe.", 404);
        return ServiceResult<TecnicoDto>.SuccessResponse(ToDto(tecnico));
    }

    public ServiceResult<TecnicoDto> CreateTecnico(CreateTecnicoDto dto)
    {
        var error = Validar(dto);
        if (error != null)
            return ServiceResult<TecnicoDto>.ErrorResponse(error);

        var tecnico = new Tecnico
        {
            Nombre = dto.Nombre.Trim(),
            Especialidad = dto.Especialidad.Trim()
        };
        _tecnicos.Add(tecnico);
        return ServiceResult<TecnicoDto>.SuccessResponse(ToDto(tecnico), "Técnico creado correctamente.", 201);
    }

    public ServiceResult<TecnicoDto> UpdateTecnico(int id, CreateTecnicoDto dto)
    {
        var tecnico = _tecnicos.GetById(id);
        if (tecnico == null)
            return ServiceResult<TecnicoDto>.ErrorResponse("El técnico no existe.", 404);

        var error = Validar(dto);
        if (error != null)
            return ServiceResult<TecnicoDto>.ErrorResponse(error);

        tecnico.Nombre = dto.Nombre.Trim();
        tecnico.Especialidad = dto.Especialidad.Trim();
        _tecnicos.Update(tecnico);
        return ServiceResult<TecnicoDto>.SuccessResponse(ToDto(tecnico), "Técnico actualizado correctamente.");
    }

    public ServiceResult<bool> DeleteTecnico(int id)
    {
        var tecnico = _tecnicos.GetById(id);
        if (tecnico == null)
            return ServiceResult<bool>.ErrorResponse("El técnico no existe.", 404);

        if (_incidencias.ExisteConTecnico(id))
            return ServiceResult<bool>.ErrorResponse("El técnico tiene incidencias asignadas y no se puede eliminar.", 409);

        _tecnicos.Remove(tecnico);
        return ServiceResult<bool>.SuccessResponse(true, "Técnico eliminado correctamente.");
    }

    private static string? Validar(CreateTecnicoDto dto)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            errores.Add("El nombre es obligatorio.");
        else if (dto.Nombre.Trim().Length < 2 || dto.Nombre.Trim().Length > 100)
            errores.Add("El nombre debe tener entre 2 y 100 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.Especialidad))
            errores.Add("La especialidad es obligatoria.");
        else if (dto.Especialidad.Trim().Length > 100)
            errores.Add("La especialidad no puede tener más de 100 caracteres.");

        return errores.Count == 0 ? null : string.Join(" ", errores);
    }

    private static TecnicoDto ToDto(Tecnico t) => new()
    {
        Id = t.Id,
        Nombre = t.Nombre,
        Especialidad = t.Especialidad
    };
}
