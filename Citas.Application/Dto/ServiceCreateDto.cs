namespace Citas.Application.Dto;

public class ServiceCreateDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public float SuggestedPrice { get; set; }
    public bool IsUnavailable { get; set; }
}