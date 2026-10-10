namespace GestorIncidencias.API.Models.Entities;

public class Incidencia
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public string Prioridad { get; set; } = "";
    public string Estado { get; set; } = "";
    public DateTime FechaReporte { get; set; }
    public string? Solucion { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public int? TecnicoId { get; set; }
    public Tecnico? Tecnico { get; set; }
}
