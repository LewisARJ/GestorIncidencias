using GestorIncidencias.Domain.Entities;

namespace GestorIncidencias.Infrastructure.Interfaces;

public interface IIncidenciaRepository : IBaseRepository<Incidencia>
{
    List<Incidencia> GetByEstado(string estado);
    bool ExisteConTecnico(int tecnicoId);
}
