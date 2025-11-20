using System.Linq.Expressions;

namespace Citas.Domain.Repositories;

public interface IRepository<T, TKey> where T : class
{
  Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
  Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
  Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
  Task<T?> FirstOrDefaultWithIncludesAsync(
    Expression<Func<T, bool>> predicate,
    CancellationToken cancellationToken = default,
    params Expression<Func<T, object>>[] includes);
  void Add(T entity);
  void Update(T entity);
  void Delete(T entity);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}