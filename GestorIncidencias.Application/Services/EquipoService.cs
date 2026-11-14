using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;
using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;

namespace GestorIncidencias.Application.Services;

public class EquipoService : IEquipoService
{
    private static readonly string[] EstadosPermitidos = { "Operativo", "En reparación", "Fuera de servicio" };

    private readonly IBaseRepository<Equipo> _equipos;
    private readonly IIncidenciaRepository _incidencias;

    public EquipoService(IBaseRepository<Equipo> equipos, IIncidenciaRepository incidencias)
    {
        _equipos = equipos;
        _incidencias = incidencias;
    }

    public ServiceResult<IEnumerable<EquipoDto>> GetAllEquipos()
    {
        var lista = _equipos.GetAll().Select(ToDto).ToList();
        return ServiceResult<IEnumerable<EquipoDto>>.SuccessResponse(lista);
    }

    public ServiceResult<EquipoDto> GetEquipoById(int id)
    {
        var equipo = _equipos.GetById(id);
        if (equipo == null)
            return ServiceResult<EquipoDto>.ErrorResponse("El equipo no existe.", 404);
        return ServiceResult<EquipoDto>.SuccessResponse(ToDto(equipo));
    }

    public ServiceResult<EquipoDto> CreateEquipo(CreateEquipoDto dto)
    {
        var error = Validar(dto, out var estado);
        if (error != null)
            return ServiceResult<EquipoDto>.ErrorResponse(error);

        if (CodigoRepetido(dto.Codigo.Trim(), 0))
            return ServiceResult<EquipoDto>.ErrorResponse("Ya existe un equipo con ese código.", 409);

        var equipo = new Equipo
        {
            Codigo = dto.Codigo.Trim(),
            Tipo = dto.Tipo.Trim(),
            Estado = estado!
        };
        _equipos.Add(equipo);
        return ServiceResult<EquipoDto>.SuccessResponse(ToDto(equipo), "Equipo creado correctamente.", 201);
    }

    public ServiceResult<EquipoDto> UpdateEquipo(int id, CreateEquipoDto dto)
    {
        var equipo = _equipos.GetById(id);
        if (equipo == null)
            return ServiceResult<EquipoDto>.ErrorResponse("El equipo no existe.", 404);

        var error = Validar(dto, out var estado);
        if (error != null)
            return ServiceResult<EquipoDto>.ErrorResponse(error);

        if (CodigoRepetido(dto.Codigo.Trim(), id))
            return ServiceResult<EquipoDto>.ErrorResponse("Ya existe un equipo con ese código.", 409);

        equipo.Codigo = dto.Codigo.Trim();
        equipo.Tipo = dto.Tipo.Trim();
        equipo.Estado = estado!;
        _equipos.Update(equipo);
        return ServiceResult<EquipoDto>.SuccessResponse(ToDto(equipo), "Equipo actualizado correctamente.");
    }

    public ServiceResult<bool> DeleteEquipo(int id)
    {
        var equipo = _equipos.GetById(id);
        if (equipo == null)
            return ServiceResult<bool>.ErrorResponse("El equipo no existe.", 404);

        if (_incidencias.GetAll().Any(i => i.EquipoId == id))
            return ServiceResult<bool>.ErrorResponse("El equipo tiene incidencias registradas y no se puede eliminar.", 409);

        _equipos.Remove(equipo);
        return ServiceResult<bool>.SuccessResponse(true, "Equipo eliminado correctamente.");
    }

    private bool CodigoRepetido(string codigo, int idActual)
    {
        return _equipos.GetAll().Any(e =>
            e.Id != idActual &&
            string.Equals(e.Codigo, codigo, StringComparison.OrdinalIgnoreCase));
    }

    private static string? Validar(CreateEquipoDto dto, out string? estadoNormalizado)
    {
        var errores = new List<string>();
        estadoNormalizado = null;

        if (string.IsNullOrWhiteSpace(dto.Codigo))
            errores.Add("El código es obligatorio.");
        else if (dto.Codigo.Trim().Length > 30)
            errores.Add("El código no puede tener más de 30 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.Tipo))
            errores.Add("El tipo es obligatorio.");
        else if (dto.Tipo.Trim().Length > 50)
            errores.Add("El tipo no puede tener más de 50 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.Estado))
            errores.Add("El estado es obligatorio.");
        else
        {
            estadoNormalizado = EstadosPermitidos.FirstOrDefault(e =>
                string.Equals(e, dto.Estado.Trim(), StringComparison.OrdinalIgnoreCase));
            if (estadoNormalizado == null)
                errores.Add("El estado debe ser Operativo, En reparación o Fuera de servicio.");
        }

        return errores.Count == 0 ? null : string.Join(" ", errores);
    }

    private static EquipoDto ToDto(Equipo e) => new()
    {
        Id = e.Id,
        Codigo = e.Codigo,
        Tipo = e.Tipo,
        Estado = e.Estado
    };
}
