using Citas.Domain.Filters;

namespace Citas.Infrastructure.Specifications;

internal class PaginationSpecification<Entity>(
    PaginationFilter pagination
  ) : Specification<Entity>
{
  public override IQueryable<Entity> Apply(IQueryable<Entity> query)
  {
    return query
     .Skip((pagination.Page - 1) * pagination.PageSize)
      .Take(pagination.PageSize);
  }
}
