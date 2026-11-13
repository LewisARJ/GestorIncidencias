using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Context;
using GestorIncidencias.Infrastructure.Core;
using GestorIncidencias.Infrastructure.Interfaces;

namespace GestorIncidencias.Infrastructure.Repositories;

public class IncidenciaRepository : BaseRepository<Incidencia>, IIncidenciaRepository
{
    public IncidenciaRepository(DataContext context) : base(context)
    {
    }

    public List<Incidencia> GetByEstado(string estado)
    {
        return _dbSet.Where(i => i.Estado == estado).ToList();
    }

    public bool ExisteConTecnico(int tecnicoId)
    {
        return _dbSet.Any(i => i.TecnicoId == tecnicoId);
    }
}
