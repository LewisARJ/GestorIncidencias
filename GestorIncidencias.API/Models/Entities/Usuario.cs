namespace GestorIncidencias.API.Models.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Departamento { get; set; } = "";
    public string Correo { get; set; } = "";
}
