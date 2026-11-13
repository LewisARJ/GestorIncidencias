using GestorIncidencias.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias.Infrastructure.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Tecnico> Tecnicos => Set<Tecnico>();
    public DbSet<Incidencia> Incidencias => Set<Incidencia>();
}
