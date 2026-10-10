namespace GestorIncidencias.API.Models.Dtos;

public class IncidenciaDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public string Prioridad { get; set; } = "";
    public string Estado { get; set; } = "";
    public DateTime FechaReporte { get; set; }
    public string? Solucion { get; set; }
    public int UsuarioId { get; set; }
    public int EquipoId { get; set; }
    public int? TecnicoId { get; set; }
}
