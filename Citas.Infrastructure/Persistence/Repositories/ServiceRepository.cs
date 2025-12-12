using Citas.Domain.Entities;
using Citas.Domain.Filters;
using Citas.Domain.Repositories;
using Citas.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Citas.Infrastructure.Persistence.Repositories;

public class ServiceRepository : BaseRepository<Service, int>, IServiceRepository
{
    public ServiceRepository(CitasDbContext db) : base(db) { }

    public async Task<List<Service>> FindAllByCompany(int companyId, PaginationFilter filter, CancellationToken ct)
    {
        var query = _set.Where(s => s.Company.Id == companyId);

        return await new PaginationSpecification<Service>(filter)
            .Apply(query)
            .ToListAsync(ct);
    }

    public Task<Service?> FindByIdAndCompany(int id, int companyId, CancellationToken ct)
    {
        return _set
            .Where(s => s.Id == id && s.Company.Id == companyId)
            .FirstOrDefaultAsync(ct);
    }
}