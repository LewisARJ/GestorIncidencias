using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly IEquipoService _service;

    public EquiposController(IEquipoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _service.GetAllEquipos();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _service.GetEquipoById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public IActionResult Create(CreateEquipoDto dto)
    {
        var result = _service.CreateEquipo(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateEquipoDto dto)
    {
        var result = _service.UpdateEquipo(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.DeleteEquipo(id);
        return StatusCode(result.StatusCode, result);
    }
}
