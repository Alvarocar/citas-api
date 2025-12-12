using Citas.Application.Dto;
using Citas.Application.Services;
using Citas.Domain.Enums;
using Citas.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ServicesController(IServiceService _serviceService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = nameof(ERolType.ADMINISTRATOR))]
    public async Task<IActionResult> Create(ServiceCreateDto dto, CancellationToken ct)
    {
        var user = GetUserTokenFromClaims();
        var result = await _serviceService.Create(dto, user, ct);
        return Created(string.Empty, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter, CancellationToken ct)
    {
        var user = GetUserTokenFromClaims();
        var services = await _serviceService.GetAll(user, filter, ct);
        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var user = GetUserTokenFromClaims();
        var service = await _serviceService.GetById(id, user, ct);
        return service is null ? NotFound() : Ok(service);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ServiceUpdateDto dto, CancellationToken ct)
    {
        var user = GetUserTokenFromClaims();
        var updated = await _serviceService.Update(id, dto, user, ct);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var user = GetUserTokenFromClaims();
        await _serviceService.Delete(id, user, ct);
        return NoContent();
    }
}
