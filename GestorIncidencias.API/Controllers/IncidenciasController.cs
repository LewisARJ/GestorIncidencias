using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase
{
    private readonly IIncidenciaService _service;

    public IncidenciasController(IIncidenciaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string? estado)
    {
        var result = _service.GetAllIncidencias(estado);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _service.GetIncidenciaById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public IActionResult Create(CreateIncidenciaDto dto)
    {
        var result = _service.CreateIncidencia(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateIncidenciaDto dto)
    {
        var result = _service.UpdateIncidencia(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.DeleteIncidencia(id);
        return StatusCode(result.StatusCode, result);
    }
}
