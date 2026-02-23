using Citas.Application.Dto;
using Citas.Application.Schedules;
using Citas.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchedulesController(
    ScheduleService scheduleService
  ) : BaseController
{

  [HttpPost]
  [Authorize(Roles = Rol.Administrator)]
  public async Task<IActionResult> Create([FromBody] ScheduleCreateDto dto, CancellationToken ct)
  {
    var user = GetUserTokenFromClaims();
    var schedule = await scheduleService.CreateOne(user, dto, ct);
    return Created($"/api/schedules/{schedule.Id}", new EmployeeScheduleDto
    {
      Id = schedule.Id,
      Name = schedule.Name,
      Ranges = dto.Ranges // Reuse the input directly (DTO parity)
    });
  }
}