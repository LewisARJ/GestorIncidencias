using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicosController : ControllerBase
{
    private static readonly List<Tecnico> _tecnicos = new();
    private static int _siguienteId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<TecnicoDto>> GetAll()
    {
        return Ok(_tecnicos.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<TecnicoDto> GetById(int id)
    {
        var tecnico = _tecnicos.FirstOrDefault(t => t.Id == id);
        if (tecnico == null) return NotFound();
        return Ok(ToDto(tecnico));
    }

    [HttpPost]
    public ActionResult<TecnicoDto> Create(CreateTecnicoDto dto)
    {
        var tecnico = new Tecnico
        {
            Id = _siguienteId++,
            Nombre = dto.Nombre,
            Especialidad = dto.Especialidad
        };
        _tecnicos.Add(tecnico);
        return CreatedAtAction(nameof(GetById), new { id = tecnico.Id }, ToDto(tecnico));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateTecnicoDto dto)
    {
        var tecnico = _tecnicos.FirstOrDefault(t => t.Id == id);
        if (tecnico == null) return NotFound();
        tecnico.Nombre = dto.Nombre;
        tecnico.Especialidad = dto.Especialidad;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var tecnico = _tecnicos.FirstOrDefault(t => t.Id == id);
        if (tecnico == null) return NotFound();
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
