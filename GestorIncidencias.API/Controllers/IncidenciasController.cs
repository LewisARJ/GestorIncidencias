using GestorIncidencias.API.Data;
using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private readonly DataContext _context;

    public IncidenciasController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<IncidenciaDto>> GetAll()
    {
        var incidencias = _context.Incidencias.ToList();
        return Ok(incidencias.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<IncidenciaDto> GetById(int id)
    {
        var incidencia = _context.Incidencias.Find(id);
        if (incidencia == null) return NotFound();
        return Ok(ToDto(incidencia));
    }

    [HttpPost]
    public ActionResult<IncidenciaDto> Create(CreateIncidenciaDto dto)
    {
        if (!_context.Usuarios.Any(u => u.Id == dto.UsuarioId))
            return BadRequest("El usuario indicado no existe.");
        if (!_context.Equipos.Any(e => e.Id == dto.EquipoId))
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
        _context.Incidencias.Add(incidencia);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = incidencia.Id }, ToDto(incidencia));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateIncidenciaDto dto)
    {
        var incidencia = _context.Incidencias.Find(id);
        if (incidencia == null) return NotFound();
        if (dto.TecnicoId != null && !_context.Tecnicos.Any(t => t.Id == dto.TecnicoId))
            return BadRequest("El técnico indicado no existe.");

        incidencia.Prioridad = dto.Prioridad;
        incidencia.Estado = dto.Estado;
        incidencia.Solucion = dto.Solucion;
        incidencia.TecnicoId = dto.TecnicoId;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var incidencia = _context.Incidencias.Find(id);
        if (incidencia == null) return NotFound();
        _context.Incidencias.Remove(incidencia);
        _context.SaveChanges();
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
