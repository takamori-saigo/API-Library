using System.Linq.Expressions;
using Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public abstract class RepositoryBase<T>: IRepositoryBase<T> where T : class
{
    private RestorDbContext _context;
    
    public RepositoryBase(RestorDbContext restorDbContext)
    {
        _context = restorDbContext;    
    }

    public IQueryable<T> GetAll(bool trackChanges)
    {
        return trackChanges? _context.Set<T>().AsTracking() :_context.Set<T>();
    }

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> predicate, bool trackChanges)
    {
        return trackChanges? _context.Set<T>().Where(predicate).AsTracking() : _context.Set<T>().Where(predicate);
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}