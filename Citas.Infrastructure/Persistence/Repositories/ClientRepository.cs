using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class ClientRepository : BaseRepository<Client, int>, IClientRepository
{
    public ClientRepository(CitasDbContext db) : base(db) { }
}