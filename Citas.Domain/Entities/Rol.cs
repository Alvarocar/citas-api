using Citas.Domain.Enums;

namespace Citas.Domain.Entities
{
    /**<summary>
     * Represents a role within the system.
     * </summary>
     */
    public class Rol
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ERolType Type { get; set; }
    }
}
