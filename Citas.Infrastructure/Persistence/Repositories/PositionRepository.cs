using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class PositionRepository : BaseRepository<Position, long>, IPositionRepository
{
    public PositionRepository(CitasDbContext db) : base(db) { }
}