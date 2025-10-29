namespace Citas.Domain.Entities
{
    /**<summary>
     * Represents a position within the organization.
     * </summary>
     */
    public class Position
    {
        public long Id { get; set; }

        public required string Name { get; set; }
    }
}
