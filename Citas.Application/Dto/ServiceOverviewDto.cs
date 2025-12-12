namespace Citas.Application.Dto;

public class ServiceOverviewDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public float SuggestedPrice { get; set; }
    public bool IsUnavailable { get; set; }
}