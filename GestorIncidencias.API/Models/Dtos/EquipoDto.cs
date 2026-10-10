namespace GestorIncidencias.API.Models.Dtos;

public class EquipoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Estado { get; set; } = "";
}
