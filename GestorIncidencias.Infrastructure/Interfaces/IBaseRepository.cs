namespace GestorIncidencias.Infrastructure.Interfaces;

public interface IBaseRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
