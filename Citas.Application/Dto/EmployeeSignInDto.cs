using System.ComponentModel.DataAnnotations;

namespace Citas.Application.Dto;

public class EmployeeSignInDto
{
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Email { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [MaxLength(30, ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Max")]
  public required string Password { get; set; }
}
