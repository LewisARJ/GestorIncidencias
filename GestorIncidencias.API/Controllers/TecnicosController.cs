using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;
using GestorIncidencias.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicosController : ControllerBase
{
    private readonly IBaseRepository<Tecnico> _tecnicos;
    private readonly IIncidenciaRepository _incidencias;

    public TecnicosController(IBaseRepository<Tecnico> tecnicos, IIncidenciaRepository incidencias)
    {
        _tecnicos = tecnicos;
        _incidencias = incidencias;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TecnicoDto>> GetAll()
    {
        return Ok(_tecnicos.GetAll().Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<TecnicoDto> GetById(int id)
    {
        var tecnico = _tecnicos.GetById(id);
        if (tecnico == null) return NotFound();
        return Ok(ToDto(tecnico));
    }

    [HttpPost]
    public ActionResult<TecnicoDto> Create(CreateTecnicoDto dto)
    {
        var tecnico = new Tecnico
        {
            Nombre = dto.Nombre,
            Especialidad = dto.Especialidad
        };
        _tecnicos.Add(tecnico);
        return CreatedAtAction(nameof(GetById), new { id = tecnico.Id }, ToDto(tecnico));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateTecnicoDto dto)
    {
        var tecnico = _tecnicos.GetById(id);
        if (tecnico == null) return NotFound();
        tecnico.Nombre = dto.Nombre;
        tecnico.Especialidad = dto.Especialidad;
        _tecnicos.Update(tecnico);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var tecnico = _tecnicos.GetById(id);
        if (tecnico == null) return NotFound();
        if (_incidencias.ExisteConTecnico(id))
            return Conflict("El técnico tiene incidencias asignadas y no se puede eliminar.");
        _tecnicos.Remove(tecnico);
        return NoContent();
    }

    private static TecnicoDto ToDto(Tecnico t) => new()
    {
        Id = t.Id,
        Nombre = t.Nombre,
        Especialidad = t.Especialidad
    };
}
