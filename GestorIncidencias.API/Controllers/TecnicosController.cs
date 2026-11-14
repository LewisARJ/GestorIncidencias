using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorIncidencias.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicosController : ControllerBase
{
    private readonly ITecnicoService _service;

    public TecnicosController(ITecnicoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _service.GetAllTecnicos();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _service.GetTecnicoById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public IActionResult Create(CreateTecnicoDto dto)
    {
        var result = _service.CreateTecnico(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreateTecnicoDto dto)
    {
        var result = _service.UpdateTecnico(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.DeleteTecnico(id);
        return StatusCode(result.StatusCode, result);
    }
}
