using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _service.GetAllUsuarios();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _service.GetUsuarioById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public IActionResult Create(CreateUsuarioDto dto)
    {
        var result = _service.CreateUsuario(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateUsuarioDto dto)
    {
        var result = _service.UpdateUsuario(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.DeleteUsuario(id);
        return StatusCode(result.StatusCode, result);
    }
}
