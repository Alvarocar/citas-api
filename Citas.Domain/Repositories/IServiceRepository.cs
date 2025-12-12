using Citas.Domain.Entities;
using Citas.Domain.Filters;

namespace Citas.Domain.Repositories;

public interface IServiceRepository : IRepository<Service, int>
{
    Task<List<Service>> FindAllByCompany(int companyId, PaginationFilter filter, CancellationToken ct);
    Task<Service?> FindByIdAndCompany(int id, int companyId, CancellationToken ct);
}