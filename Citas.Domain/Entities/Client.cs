namespace Citas.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    
        public required string PhoneNumber { get; set; }
    }
}
