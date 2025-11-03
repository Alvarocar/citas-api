using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CitasDbContext _db;
    public UnitOfWork(CitasDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
        return new EfCoreTransaction(tx);
    }
}

