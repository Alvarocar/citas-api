using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class RolRepository : BaseRepository<Rol, int>, IRolRepository
{
    public RolRepository(CitasDbContext db) : base(db) { }
}