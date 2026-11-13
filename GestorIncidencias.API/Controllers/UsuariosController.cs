using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;
using GestorIncidencias.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _usuarios;

    public UsuariosController(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UsuarioDto>> GetAll()
    {
        return Ok(_usuarios.GetAll().Select(ToDto));
    }

    [HttpGet("{id}")]
    public ActionResult<UsuarioDto> GetById(int id)
    {
        var usuario = _usuarios.GetById(id);
        if (usuario == null) return NotFound();
        return Ok(ToDto(usuario));
    }

    [HttpPost]
    public ActionResult<UsuarioDto> Create(CreateUsuarioDto dto)
    {
        if (_usuarios.GetByCorreo(dto.Correo) != null)
            return Conflict("Ya existe un usuario con ese correo.");

        var usuario = new Usuario
        {
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
        var usuario = _usuarios.GetById(id);
        if (usuario == null) return NotFound();

        var otro = _usuarios.GetByCorreo(dto.Correo);
        if (otro != null && otro.Id != id)
            return Conflict("Ya existe un usuario con ese correo.");

        usuario.Nombre = dto.Nombre;
        usuario.Departamento = dto.Departamento;
        usuario.Correo = dto.Correo;
        _usuarios.Update(usuario);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var usuario = _usuarios.GetById(id);
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
