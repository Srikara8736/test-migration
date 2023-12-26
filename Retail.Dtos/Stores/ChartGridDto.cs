namespace Retail.DTOs.Stores;

public class ChartGridDto
{

    public string AreaType { get; set; }

    public Guid AreaTypeId { get; set; }

    public decimal TotalArea { get; set; }
    public decimal? TotalAreaPercentage { get; set; }

    public List<CategoryGridDto> Categories { get; set; } = new();
}


public class CategoryGridDto
{

    public Guid CategoryId { get; set; }
    public string Category { get; set; }

    public decimal TotalArea { get; set; }
    public decimal? TotalAreaPercentage { get; set; }

    public List<SpaceGridDto> Spaces { get; set; } = new();

}


public class SpaceGridDto
{

    public Guid SpaceId { get; set; }
    public string Space { get; set; }
    public string Unit { get; set; }
    public decimal Atricles { get; set; }
    public decimal Pieces { get; set; }
    public decimal Area { get; set; }

}