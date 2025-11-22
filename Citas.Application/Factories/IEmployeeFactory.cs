using Citas.Application.Dto;
using Citas.Domain.Entities;

namespace Citas.Application.Factories;

public interface IEmployeeFactory
{
  Employee CreateAdmin(EmployeeCreateAdminDto dto, Rol rol, Company company);

  Employee Create(EmployeeCreateDto dto, Rol rol, Company company);

  UserTokenDto CreateToken(Employee employee);

  UserTokenDto? CreateToken(string token);
}
