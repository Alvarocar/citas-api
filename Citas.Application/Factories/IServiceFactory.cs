using Citas.Application.Dto;
using Citas.Domain.Entities;

namespace Citas.Application.Factories;

public interface IServiceFactory
{
    Service Create(ServiceCreateDto dto, Company company);
    ServiceOverviewDto CreateOverview(Service service);
}