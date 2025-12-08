using Citas.Domain.Enums;

namespace Citas.Application.Dto;

public class UserTokenDto
{
  public int Id { get; set; }
  public string? Email { get; set; } = null!;
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public required ERolType Role { get; set; }
}
