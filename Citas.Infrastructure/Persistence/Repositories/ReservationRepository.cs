using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class ReservationRepository : BaseRepository<Reservation, int>, IReservationRepository
{
    public ReservationRepository(CitasDbContext db) : base(db) { }
}