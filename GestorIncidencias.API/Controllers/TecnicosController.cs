using GestorIncidencias.API.Data;
using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicosController : ControllerBase
{
    private readonly DataContext _context;

    public TecnicosController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TecnicoDto>> GetAll()
    {
        var tecnicos = _context.Tecnicos.ToList();
        return Ok(tecnicos.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<TecnicoDto> GetById(int id)
    {
        var tecnico = _context.Tecnicos.Find(id);
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
        _context.Tecnicos.Add(tecnico);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = tecnico.Id }, ToDto(tecnico));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateTecnicoDto dto)
    {
        var tecnico = _context.Tecnicos.Find(id);
        if (tecnico == null) return NotFound();
        tecnico.Nombre = dto.Nombre;
        tecnico.Especialidad = dto.Especialidad;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var tecnico = _context.Tecnicos.Find(id);
        if (tecnico == null) return NotFound();
        if (_context.Incidencias.Any(i => i.TecnicoId == id))
            return Conflict("El técnico tiene incidencias asignadas y no se puede eliminar.");
        _context.Tecnicos.Remove(tecnico);
        _context.SaveChanges();
        return NoContent();
    }

    private static TecnicoDto ToDto(Tecnico t) => new()
    {
        Id = t.Id,
        Nombre = t.Nombre,
        Especialidad = t.Especialidad
    };
}
