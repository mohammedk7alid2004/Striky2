namespace Striky.Api.Repository;
using Striky.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Striky.Api.Interface.IRepository;

public class Repository<T> : IRepository<T> where T : class
{
    protected Db16821Context _context;

    public Repository(Db16821Context context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public List<string> GetDistinct(Expression<Func<T, string>> col)
    {
        return _context.Set<T>()
                     .Select(col)
                     .Distinct()
                     .ToList();
    }

    public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query.SingleOrDefault(criteria);
    }

    public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var incluse in includes)
                query = query.Include(incluse);

        return await query.SingleOrDefaultAsync(criteria);
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query.Where(criteria).ToList();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
    {
        return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
        Expression<Func<T, object>> orderBy = null, bool IsDesc = false)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        if (orderBy != null)
        {
            query = IsDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return query.ToList();
    }

    public IEnumerable<T> FindWithFilters(Expression<Func<T, bool>> criteria = null,
        string sortColumn = null, string sortColumnDirection = null,
        int? skip = null, int? take = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (criteria != null)
            query = query.Where(criteria);

        if (!string.IsNullOrEmpty(sortColumn))
        {
            if (!string.IsNullOrEmpty(sortColumnDirection)
                && sortColumnDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
            }
            else
            {
                query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
            }
        }

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return query.ToList();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.Where(criteria).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip)
    {
        return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
        Expression<Func<T, object>> orderBy = null, bool IsDesc = false)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria);

        if (take.HasValue)
            query = query.Take(take.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (orderBy != null)
        {
            query = IsDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return await query.ToListAsync();
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public T Update(T entity)
    {
        _context.Update(entity);
        return entity;
    }

    public bool UpdateRange(IEnumerable<T> entities)
    {
        _context.UpdateRange(entities);
        return true;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public int Count()
    {
        return _context.Set<T>().Count();
    }

    public int Count(Expression<Func<T, bool>> criteria)
    {
        return _context.Set<T>().Count(criteria);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<T>().CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
    {
        return await _context.Set<T>().CountAsync(criteria);
    }

    public async Task<long> MaxAsync(Expression<Func<T, object>> column)
    {
        return Convert.ToInt64(await _context.Set<T>().MaxAsync(column));
    }

    public async Task<long> MaxAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> column)
    {
        return Convert.ToInt64(await _context.Set<T>().Where(criteria).MaxAsync(column));
    }

    public long Max(Expression<Func<T, object>> column)
    {
        return Convert.ToInt64(_context.Set<T>().Max(column));
    }

    public long Max(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> column)
    {
        return Convert.ToInt64(_context.Set<T>().Where(criteria).Max(column));
    }

    public bool IsExist(Expression<Func<T, bool>> criteria)
    {
        return _context.Set<T>().Any(criteria);
    }

    public T Last(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy)
    {
        return _context.Set<T>().OrderByDescending(orderBy).FirstOrDefault(criteria);
    }
}