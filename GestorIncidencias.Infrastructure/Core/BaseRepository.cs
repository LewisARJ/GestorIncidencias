using GestorIncidencias.Infrastructure.Context;
using GestorIncidencias.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias.Infrastructure.Core;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public virtual T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}
