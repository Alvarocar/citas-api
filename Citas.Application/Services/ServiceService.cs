using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Filters;
using Citas.Domain.Repositories;

namespace Citas.Application.Services;

public class ServiceService(
    IServiceRepository _serviceRepository,
    ICompanyRepository _companyRepository,
    IServiceFactory _serviceFactory
) : IServiceService
{
    public async Task<ServiceOverviewDto> Create(
        ServiceCreateDto dto,
        UserTokenDto user,
        CancellationToken ct)
    {
        var company = await _companyRepository.GetByIdAsync(user.CompanyId, ct);
        if (company == null) throw new NotFoundException();

        var newService = _serviceFactory.Create(dto, company);
        _serviceRepository.Add(newService);

        await _serviceRepository.SaveChangesAsync(ct);
        return _serviceFactory.CreateOverview(newService);
    }

    public async Task<List<ServiceOverviewDto>> GetAll(UserTokenDto user, PaginationFilter filter, CancellationToken ct)
    {
        var services = await _serviceRepository.FindAllByCompany(user.CompanyId, filter, ct);
        return services
            .Select(_serviceFactory.CreateOverview).ToList();
    }

    public async Task<ServiceOverviewDto?> GetById(int id, UserTokenDto user, CancellationToken ct)
    {
        var service = await _serviceRepository.FindByIdAndCompany(id, user.CompanyId, ct);
        if (service == null) return null;

        return _serviceFactory.CreateOverview(service);
    }

    public async Task<ServiceOverviewDto> Update(
        int id,
        ServiceUpdateDto dto,
        UserTokenDto user,
        CancellationToken ct)
    {
        var service = await _serviceRepository.FindByIdAndCompany(id, user.CompanyId, ct);
        if (service == null) throw new NotFoundException("Servicio no encontrado.");

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.SuggestedPrice = dto.SuggestedPrice;
        service.IsUnavailable = dto.IsUnavailable;

        await _serviceRepository.SaveChangesAsync(ct);
        return _serviceFactory.CreateOverview(service);
    }

    public async Task Delete(int id, UserTokenDto user, CancellationToken ct)
    {
        var service = await _serviceRepository.FindByIdAndCompany(id, user.CompanyId, ct);
        if (service == null) throw new NotFoundException("Servicio no encontrado.");

        _serviceRepository.Delete(service);
        await _serviceRepository.SaveChangesAsync(ct);
    }
}