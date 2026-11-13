using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Context;
using GestorIncidencias.Infrastructure.Core;
using GestorIncidencias.Infrastructure.Interfaces;

namespace GestorIncidencias.Infrastructure.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(DataContext context) : base(context)
    {
    }

    public Usuario? GetByCorreo(string correo)
    {
        return _dbSet.FirstOrDefault(u => u.Correo == correo);
    }
}
