using Citas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Citas.Infrastructure.Persistence.Repositories;

public class BaseRepository<T, TKey> : IRepository<T, TKey> where T : class
{
  protected readonly CitasDbContext _db;
  protected readonly DbSet<T> _set;

  public BaseRepository(CitasDbContext db)
  {
    _db = db;
    _set = _db.Set<T>();
  }

  public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
  {
    var found = await _set.FindAsync([id], cancellationToken);
    return found;
  }

  public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
      => await _set.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

  public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
        => await _set.AsNoTracking().ToListAsync(cancellationToken);

  public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
      => await _set.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

  public virtual void Add(T entity)
  {
    _set.Add(entity);
  }

  public virtual void Update(T entity)
  {
    _set.Update(entity);
  }

  public virtual void Delete(T entity)
  {
    _set.Remove(entity);
  }

  public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
      => _db.SaveChangesAsync(cancellationToken);

  public virtual async Task<T?> FirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
  {
    IQueryable<T> query = _set;

    foreach (var include in includes)
    {
      query = query.Include(include);
    }

    return await query.FirstOrDefaultAsync(predicate, cancellationToken);
  }
}