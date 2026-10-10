using GestorIncidencias.API.Data;
using GestorIncidencias.API.Models.Dtos;
using GestorIncidencias.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly DataContext _context;

    public UsuariosController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UsuarioDto>> GetAll()
    {
        var usuarios = _context.Usuarios.ToList();
        return Ok(usuarios.Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<UsuarioDto> GetById(int id)
    {
        var usuario = _context.Usuarios.Find(id);
        if (usuario == null) return NotFound();
        return Ok(ToDto(usuario));
    }

    [HttpPost]
    public ActionResult<UsuarioDto> Create(CreateUsuarioDto dto)
    {
        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Departamento = dto.Departamento,
            Correo = dto.Correo
        };
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, ToDto(usuario));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateUsuarioDto dto)
    {
        var usuario = _context.Usuarios.Find(id);
        if (usuario == null) return NotFound();
        usuario.Nombre = dto.Nombre;
        usuario.Departamento = dto.Departamento;
        usuario.Correo = dto.Correo;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var usuario = _context.Usuarios.Find(id);
        if (usuario == null) return NotFound();
        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();
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
