using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;
using GestorIncidencias.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private readonly IIncidenciaRepository _incidencias;
    private readonly IUsuarioRepository _usuarios;
    private readonly IBaseRepository<Equipo> _equipos;
    private readonly IBaseRepository<Tecnico> _tecnicos;

    public IncidenciasController(
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

    [HttpGet]
    public ActionResult<IEnumerable<IncidenciaDto>> GetAll([FromQuery] string? estado)
    {
        var lista = string.IsNullOrWhiteSpace(estado)
            ? _incidencias.GetAll()
            : _incidencias.GetByEstado(estado);
        return Ok(lista.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<IncidenciaDto> GetById(int id)
    {
        var incidencia = _incidencias.GetById(id);
        if (incidencia == null) return NotFound();
        return Ok(ToDto(incidencia));
    }

    [HttpPost]
    public ActionResult<IncidenciaDto> Create(CreateIncidenciaDto dto)
    {
        if (_usuarios.GetById(dto.UsuarioId) == null)
            return BadRequest("El usuario indicado no existe.");
        if (_equipos.GetById(dto.EquipoId) == null)
            return BadRequest("El equipo indicado no existe.");

        var incidencia = new Incidencia
        {
            Descripcion = dto.Descripcion,
            Prioridad = dto.Prioridad,
            Estado = "Abierta",
            FechaReporte = DateTime.Now,
            UsuarioId = dto.UsuarioId,
            EquipoId = dto.EquipoId
        };
        _incidencias.Add(incidencia);
        return CreatedAtAction(nameof(GetById), new { id = incidencia.Id }, ToDto(incidencia));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateIncidenciaDto dto)
    {
        var incidencia = _incidencias.GetById(id);
        if (incidencia == null) return NotFound();
        if (dto.TecnicoId != null && _tecnicos.GetById(dto.TecnicoId.Value) == null)
            return BadRequest("El técnico indicado no existe.");

        incidencia.Prioridad = dto.Prioridad;
        incidencia.Estado = dto.Estado;
        incidencia.Solucion = dto.Solucion;
        incidencia.TecnicoId = dto.TecnicoId;
        _incidencias.Update(incidencia);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var incidencia = _incidencias.GetById(id);
        if (incidencia == null) return NotFound();
        _incidencias.Remove(incidencia);
        return NoContent();
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
