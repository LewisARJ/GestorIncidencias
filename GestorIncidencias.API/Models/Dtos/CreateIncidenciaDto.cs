namespace GestorIncidencias.API.Models.Dtos;

public class CreateIncidenciaDto
{
    public string Descripcion { get; set; } = "";
    public string Prioridad { get; set; } = "";
    public int UsuarioId { get; set; }
    public int EquipoId { get; set; }
}
