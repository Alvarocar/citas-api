using System.ComponentModel.DataAnnotations;

namespace Citas.Application.Dto;

public class CompanyCreateDto
{
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Name { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string Address { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  public required string PhoneNumber { get; set; }
  [Required(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "Required")]
  [EmailAddress(ErrorMessageResourceType = typeof(Resources.Validations), ErrorMessageResourceName = "EmailAddress")]
  public required string Email { get; set; }
}
