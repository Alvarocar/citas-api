using Citas.Domain.Entities;

namespace Citas.Domain.Repositories;

public interface ICompanyRepository : IRepository<Company, int>
{
    // Aquí puedes añadir métodos específicos de Company si los necesitas.
}