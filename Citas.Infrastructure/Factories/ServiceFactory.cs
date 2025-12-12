using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Domain.Entities;

namespace Citas.Infrastructure.Factories;

public class ServiceFactory : IServiceFactory
{
    public Service Create(ServiceCreateDto dto, Company company)
    {
        return new Service
        {
            Name = dto.Name,
            Description = dto.Description,
            SuggestedPrice = dto.SuggestedPrice,
            IsUnavailable = false,
            Company = company,
        };
    }

    public ServiceOverviewDto CreateOverview(Service service)
    {
        return new ServiceOverviewDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            SuggestedPrice = service.SuggestedPrice,
            IsUnavailable = service.IsUnavailable,
        };
    }
}