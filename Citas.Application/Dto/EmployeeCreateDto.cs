using Citas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Citas.Application.Dto;

public class EmployeeCreateDto
{
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Firstname { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Lastname { get; set; }
  [EmailAddress(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "EmailAddress")]
  public string? Email { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Max")]
  public required string PhoneNumber { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [EnumDataType(typeof(ERolType), ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Invalid")]
  public required ERolType RoleType { get; set; }
}
