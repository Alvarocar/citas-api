using System.ComponentModel.DataAnnotations;

namespace Citas.Application.Dto;


public class EmployeeCreateAdminDto
{
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Firstname { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Lastname { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [EmailAddress(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "EmailAddress")]
  public required string Email { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [MaxLength(30, ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Max")]
  public required string Password { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Max")]
  public required string PhoneNumber { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required CompanyCreateDto Company { get; set; }
}
