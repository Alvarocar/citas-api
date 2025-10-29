namespace Citas.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        
        public required string Description { get; set; }
        
        public required float SuggestedPrice { get; set; }

        public bool IsUnavailable { get; set; }

        public required Company Company { get; set; }
    }
}
