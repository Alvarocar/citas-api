using Microsoft.EntityFrameworkCore.Storage;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence;

internal sealed class EfCoreTransaction : ITransaction
{
    private readonly IDbContextTransaction _tx;

    public EfCoreTransaction(IDbContextTransaction tx) => _tx = tx;

    public Task CommitAsync(CancellationToken cancellationToken = default) => _tx.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) => _tx.RollbackAsync(cancellationToken);

    public ValueTask DisposeAsync() => _tx.DisposeAsync();
}