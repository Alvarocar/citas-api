namespace Citas.Infrastructure.Specifications;

internal abstract class Specification<Entity>
{
  public abstract IQueryable<Entity> Apply(IQueryable<Entity> query);
}

internal class CombinedSpecification<T> : Specification<T>
{
  private readonly Specification<T>[] _specifications;

  public CombinedSpecification(params Specification<T>[] specifications)
  {
    _specifications = specifications;
  }

  public override IQueryable<T> Apply(IQueryable<T> query)
  {
    foreach (var specification in _specifications)
    {
      query = specification.Apply(query);
    }

    return query;
  }
}