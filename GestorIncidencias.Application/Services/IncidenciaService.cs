using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;
using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;

namespace GestorIncidencias.Application.Services;

public class IncidenciaService : IIncidenciaService
{
    private static readonly string[] Prioridades = { "Baja", "Media", "Alta" };
    private static readonly string[] Estados = { "Abierta", "En proceso", "Cerrada" };

    private readonly IIncidenciaRepository _incidencias;
    private readonly IUsuarioRepository _usuarios;
    private readonly IBaseRepository<Equipo> _equipos;
    private readonly IBaseRepository<Tecnico> _tecnicos;

    public IncidenciaService(
        IIncidenciaRepository incidencias,
        IUsuarioRepository usuarios,
        IBaseRepository<Equipo> equipos,
        IBaseRepository<Tecnico> tecnicos)
    {
        _incidencias = incidencias;
        _usuarios = usuarios;
        _equipos = equipos;
        _tecnicos = tecnicos;
    }

    public ServiceResult<IEnumerable<IncidenciaDto>> GetAllIncidencias(string? estado)
    {
        List<Incidencia> lista;

        if (string.IsNullOrWhiteSpace(estado))
        {
            lista = _incidencias.GetAll();
        }
        else
        {
            var estadoValido = Canonico(estado, Estados);
            if (estadoValido == null)
                return ServiceResult<IEnumerable<IncidenciaDto>>.ErrorResponse("El estado debe ser Abierta, En proceso o Cerrada.");
            lista = _incidencias.GetByEstado(estadoValido);
        }

        var dtos = lista.Select(ToDto).ToList();
        return ServiceResult<IEnumerable<IncidenciaDto>>.SuccessResponse(dtos);
    }

    public ServiceResult<IncidenciaDto> GetIncidenciaById(int id)
    {
        var incidencia = _incidencias.GetById(id);
        if (incidencia == null)
            return ServiceResult<IncidenciaDto>.ErrorResponse("La incidencia no existe.", 404);
        return ServiceResult<IncidenciaDto>.SuccessResponse(ToDto(incidencia));
    }

    public ServiceResult<IncidenciaDto> CreateIncidencia(CreateIncidenciaDto dto)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Descripcion))
            errores.Add("La descripción es obligatoria.");
        else if (dto.Descripcion.Trim().Length < 5 || dto.Descripcion.Trim().Length > 500)
            errores.Add("La descripción debe tener entre 5 y 500 caracteres.");

        var prioridad = Canonico(dto.Prioridad, Prioridades);
        if (prioridad == null)
            errores.Add("La prioridad debe ser Baja, Media o Alta.");

        if (dto.UsuarioId <= 0)
            errores.Add("El usuario es obligatorio.");
        if (dto.EquipoId <= 0)
            errores.Add("El equipo es obligatorio.");

        if (errores.Count > 0)
            return ServiceResult<IncidenciaDto>.ErrorResponse(string.Join(" ", errores));

        if (_usuarios.GetById(dto.UsuarioId) == null)
            return ServiceResult<IncidenciaDto>.ErrorResponse("El usuario indicado no existe.");
        if (_equipos.GetById(dto.EquipoId) == null)
            return ServiceResult<IncidenciaDto>.ErrorResponse("El equipo indicado no existe.");

        var incidencia = new Incidencia
        {
            Descripcion = dto.Descripcion.Trim(),
            Prioridad = prioridad!,
            Estado = "Abierta",
            FechaReporte = DateTime.Now,
            UsuarioId = dto.UsuarioId,
            EquipoId = dto.EquipoId
        };
        _incidencias.Add(incidencia);
        return ServiceResult<IncidenciaDto>.SuccessResponse(ToDto(incidencia), "Incidencia creada correctamente.", 201);
    }

    public ServiceResult<IncidenciaDto> UpdateIncidencia(int id, UpdateIncidenciaDto dto)
    {
        var incidencia = _incidencias.GetById(id);
        if (incidencia == null)
            return ServiceResult<IncidenciaDto>.ErrorResponse("La incidencia no existe.", 404);

        var errores = new List<string>();

        var prioridad = Canonico(dto.Prioridad, Prioridades);
        if (prioridad == null)
            errores.Add("La prioridad debe ser Baja, Media o Alta.");

        var estado = Canonico(dto.Estado, Estados);
        if (estado == null)
            errores.Add("El estado debe ser Abierta, En proceso o Cerrada.");

        var solucion = string.IsNullOrWhiteSpace(dto.Solucion) ? null : dto.Solucion.Trim();
        if (solucion != null && solucion.Length > 1000)
            errores.Add("La solución no puede tener más de 1000 caracteres.");
        if (estado == "Cerrada" && solucion == null)
            errores.Add("La solución es obligatoria para cerrar una incidencia.");

        if (dto.TecnicoId != null && dto.TecnicoId <= 0)
            errores.Add("El técnico indicado no es válido.");

        if (errores.Count > 0)
            return ServiceResult<IncidenciaDto>.ErrorResponse(string.Join(" ", errores));

        if (dto.TecnicoId != null && _tecnicos.GetById(dto.TecnicoId.Value) == null)
            return ServiceResult<IncidenciaDto>.ErrorResponse("El técnico indicado no existe.");

        incidencia.Prioridad = prioridad!;
        incidencia.Estado = estado!;
        incidencia.Solucion = solucion;
        incidencia.TecnicoId = dto.TecnicoId;
        _incidencias.Update(incidencia);
        return ServiceResult<IncidenciaDto>.SuccessResponse(ToDto(incidencia), "Incidencia actualizada correctamente.");
    }

    public ServiceResult<bool> DeleteIncidencia(int id)
    {
        var incidencia = _incidencias.GetById(id);
        if (incidencia == null)
            return ServiceResult<bool>.ErrorResponse("La incidencia no existe.", 404);

        _incidencias.Remove(incidencia);
        return ServiceResult<bool>.SuccessResponse(true, "Incidencia eliminada correctamente.");
    }

    private static string? Canonico(string? valor, string[] permitidos)
    {
        if (string.IsNullOrWhiteSpace(valor)) return null;
        return permitidos.FirstOrDefault(p =>
            string.Equals(p, valor.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private static IncidenciaDto ToDto(Incidencia i) => new()
    {
        Id = i.Id,
        Descripcion = i.Descripcion,
        Prioridad = i.Prioridad,
        Estado = i.Estado,
        FechaReporte = i.FechaReporte,
        Solucion = i.Solucion,
        UsuarioId = i.UsuarioId,
        EquipoId = i.EquipoId,
        TecnicoId = i.TecnicoId
    };
}
