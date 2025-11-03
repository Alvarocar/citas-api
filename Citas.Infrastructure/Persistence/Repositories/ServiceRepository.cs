using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class ServiceRepository : BaseRepository<Service, int>, IServiceRepository
{
    public ServiceRepository(CitasDbContext db) : base(db) { }
}