namespace GestorIncidencias.API.Models.Dtos;

public class UpdateIncidenciaDto
{
    public string Prioridad { get; set; } = "";
    public string Estado { get; set; } = "";
    public string? Solucion { get; set; }
    public int? TecnicoId { get; set; }
}
