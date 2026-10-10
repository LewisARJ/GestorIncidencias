namespace GestorIncidencias.API.Models.Dtos;

public class CreateUsuarioDto
{
    public string Nombre { get; set; } = "";
    public string Departamento { get; set; } = "";
    public string Correo { get; set; } = "";
}
