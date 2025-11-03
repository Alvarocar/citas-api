using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class CompanyRepository : BaseRepository<Company, int>, ICompanyRepository
{
    public CompanyRepository(CitasDbContext db) : base(db) { }

}