using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;
using GestorIncidencias.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly IBaseRepository<Equipo> _equipos;

    public EquiposController(IBaseRepository<Equipo> equipos)
    {
        _equipos = equipos;
    }

    [HttpGet]
    public ActionResult<IEnumerable<EquipoDto>> GetAll()
    {
        return Ok(_equipos.GetAll().Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<EquipoDto> GetById(int id)
    {
        var equipo = _equipos.GetById(id);
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
        _equipos.Add(equipo);
        return CreatedAtAction(nameof(GetById), new { id = equipo.Id }, ToDto(equipo));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateEquipoDto dto)
    {
        var equipo = _equipos.GetById(id);
        if (equipo == null) return NotFound();
        equipo.Codigo = dto.Codigo;
        equipo.Tipo = dto.Tipo;
        equipo.Estado = dto.Estado;
        _equipos.Update(equipo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var equipo = _equipos.GetById(id);
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
