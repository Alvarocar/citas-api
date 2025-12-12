using Citas.Application.Dto;
using Citas.Domain.Filters;

namespace Citas.Application.Services;

public interface IServiceService
{
    Task<ServiceOverviewDto> Create(
        ServiceCreateDto dto,
        UserTokenDto user,
        CancellationToken ct);

    Task<List<ServiceOverviewDto>> GetAll(
        UserTokenDto user,
        PaginationFilter filter,
        CancellationToken ct);

    Task<ServiceOverviewDto?> GetById(
        int id,
        UserTokenDto user,
        CancellationToken ct);

    Task<ServiceOverviewDto> Update(
        int id,
        ServiceUpdateDto dto,
        UserTokenDto user,
        CancellationToken ct);

    Task Delete(
        int id,
        UserTokenDto user,
        CancellationToken ct);
}