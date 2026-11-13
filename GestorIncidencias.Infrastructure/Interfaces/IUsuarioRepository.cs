using GestorIncidencias.Domain.Entities;

namespace GestorIncidencias.Infrastructure.Interfaces;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Usuario? GetByCorreo(string correo);
}
