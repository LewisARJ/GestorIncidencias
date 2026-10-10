using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private static readonly List<Incidencia> _incidencias = new();
    private static int _siguienteId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<IncidenciaDto>> GetAll()
    {
        return Ok(_incidencias.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<IncidenciaDto> GetById(int id)
    {
        var incidencia = _incidencias.FirstOrDefault(i => i.Id == id);
        if (incidencia == null) return NotFound();
        return Ok(ToDto(incidencia));
    }

    [HttpPost]
    public ActionResult<IncidenciaDto> Create(CreateIncidenciaDto dto)
    {
        var incidencia = new Incidencia
        {
            Id = _siguienteId++,
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
        var incidencia = _incidencias.FirstOrDefault(i => i.Id == id);
        if (incidencia == null) return NotFound();
        incidencia.Prioridad = dto.Prioridad;
        incidencia.Estado = dto.Estado;
        incidencia.Solucion = dto.Solucion;
        incidencia.TecnicoId = dto.TecnicoId;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var incidencia = _incidencias.FirstOrDefault(i => i.Id == id);
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
