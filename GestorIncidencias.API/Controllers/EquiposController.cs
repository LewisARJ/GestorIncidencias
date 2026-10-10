using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private static readonly List<Equipo> _equipos = new();
    private static int _siguienteId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<EquipoDto>> GetAll()
    {
        return Ok(_equipos.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<EquipoDto> GetById(int id)
    {
        var equipo = _equipos.FirstOrDefault(e => e.Id == id);
        if (equipo == null) return NotFound();
        return Ok(ToDto(equipo));
    }

    [HttpPost]
    public ActionResult<EquipoDto> Create(CreateEquipoDto dto)
    {
        var equipo = new Equipo
        {
            Id = _siguienteId++,
            Codigo = dto.Codigo,
            Tipo = dto.Tipo,
            Estado = dto.Estado
        };
        _equipos.Add(equipo);
        return CreatedAtAction(nameof(GetById), new { id = equipo.Id }, ToDto(equipo));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateEquipoDto dto)
    {
        var equipo = _equipos.FirstOrDefault(e => e.Id == id);
        if (equipo == null) return NotFound();
        equipo.Codigo = dto.Codigo;
        equipo.Tipo = dto.Tipo;
        equipo.Estado = dto.Estado;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var equipo = _equipos.FirstOrDefault(e => e.Id == id);
        if (equipo == null) return NotFound();
        _equipos.Remove(equipo);
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
