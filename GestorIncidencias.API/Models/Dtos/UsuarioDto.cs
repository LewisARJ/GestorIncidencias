namespace GestorIncidencias.API.Models.Dtos;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Departamento { get; set; } = "";
    public string Correo { get; set; } = "";
}
