namespace GestorIncidencias.API.Models.Entities;

public class Equipo
{
    public int Id { get; set; }
    public string Codigo { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Estado { get; set; } = "";
}
