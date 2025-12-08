using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Application.Services;
using Citas.Domain.Entities;
using Citas.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Citas.Infrastructure.Factories;

public class EmployeeFactory(
    IPasswordHasherService _passwordHasher
  ) : IEmployeeFactory
{
  public Employee CreateAdmin(EmployeeCreateAdminDto dto, Rol rol, Company company)
  {
    return new Employee
    {
      FirstName = dto.Firstname,
      LastName = dto.Lastname,
      Email = dto.Email.ToLower(),
      Password = _passwordHasher.HashPassword(dto.Password),
      IsActive = true,
      HasAccount = true,
      PhoneNumber = dto.PhoneNumber,
      Rol = rol,
      Company = company,
    };
  }

  public Employee Create(EmployeeCreateDto dto, Rol rol, Company company)
  {
    return new Employee
    {
      FirstName = dto.Firstname,
      LastName = dto.Lastname,
      PhoneNumber = dto.PhoneNumber,
      Email = dto.Email?.ToLower(),
      IsActive = true,
      HasAccount = dto.Email != null,
      Rol = rol,
      Company = company,
    };
  }

  public UserTokenDto CreateToken(Employee employee)
  {
    return new UserTokenDto
    {
      Id = employee!.Id,
      FirstName = employee.FirstName,
      LastName = employee.LastName,
      Email = employee.Email!,
      Role = employee.Rol.Type,
    };
  }

  public UserTokenDto? CreateToken(string rawToken)
  {
    try
    {
      var token = new JwtSecurityTokenHandler().ReadJwtToken(rawToken);
      var id = int.Parse(token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

      if (id == 0)
        return null;

      var email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;

      if (string.IsNullOrEmpty(email))
        return null;

      var firstName = token.Claims.First(c => c.Type == "firstName").Value;
      var lastName = token.Claims.First(c => c.Type == "lastName").Value;

      var rol = Enum.Parse<ERolType>(token.Claims.First(c => c.Type == ClaimTypes.Role).Value);

      return new UserTokenDto
      {
        Id = id,
        Email = email,
        FirstName = firstName,
        LastName = lastName,
        Role = rol,
      };
    }
    catch
    {
      return null;
    }
  }
}
