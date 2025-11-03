using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Citas.Domain.Repositories;

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

    public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
        => await _set.AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _set.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public virtual Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Add(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}