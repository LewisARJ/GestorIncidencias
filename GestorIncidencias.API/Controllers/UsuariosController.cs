using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private static readonly List<Usuario> _usuarios = new();
    private static int _siguienteId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<UsuarioDto>> GetAll()
    {
        return Ok(_usuarios.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<UsuarioDto> GetById(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario == null) return NotFound();
        return Ok(ToDto(usuario));
    }

    [HttpPost]
    public ActionResult<UsuarioDto> Create(CreateUsuarioDto dto)
    {
        var usuario = new Usuario
        {
            Id = _siguienteId++,
            Nombre = dto.Nombre,
            Departamento = dto.Departamento,
            Correo = dto.Correo
        };
        _usuarios.Add(usuario);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, ToDto(usuario));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateUsuarioDto dto)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario == null) return NotFound();
        usuario.Nombre = dto.Nombre;
        usuario.Departamento = dto.Departamento;
        usuario.Correo = dto.Correo;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario == null) return NotFound();
        _usuarios.Remove(usuario);
        return NoContent();
    }

    private static UsuarioDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        Departamento = u.Departamento,
        Correo = u.Correo
    };
}
