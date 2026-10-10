using GestorIncidencias.API.Data;
using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly DataContext _context;

    public EquiposController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<EquipoDto>> GetAll()
    {
        var equipos = _context.Equipos.ToList();
        return Ok(equipos.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<EquipoDto> GetById(int id)
    {
        var equipo = _context.Equipos.Find(id);
        if (equipo == null) return NotFound();
        return Ok(ToDto(equipo));
    }

    [HttpPost]
    public ActionResult<EquipoDto> Create(CreateEquipoDto dto)
    {
        var equipo = new Equipo
        {
            Codigo = dto.Codigo,
            Tipo = dto.Tipo,
            Estado = dto.Estado
        };
        _context.Equipos.Add(equipo);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = equipo.Id }, ToDto(equipo));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateEquipoDto dto)
    {
        var equipo = _context.Equipos.Find(id);
        if (equipo == null) return NotFound();
        equipo.Codigo = dto.Codigo;
        equipo.Tipo = dto.Tipo;
        equipo.Estado = dto.Estado;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var equipo = _context.Equipos.Find(id);
        if (equipo == null) return NotFound();
        _context.Equipos.Remove(equipo);
        _context.SaveChanges();
        return NoContent();
    }

    private static EquipoDto ToDto(Equipo e) => new()
    {
        Id = e.Id,
        Codigo = e.Codigo,
        Tipo = e.Tipo,
        Estado = e.Estado
    };
}
