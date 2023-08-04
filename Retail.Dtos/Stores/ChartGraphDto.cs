namespace Retail.DTOs.Stores;

public class ChartGraphDto
{
    public string ChartTitle { get; set; }
    public string ChartType { get; set; }
    public List<ChartItemDto> chartItems { get; set; } = new();

   
}

public class ChartItemDto
{
    public string Key { get; set; }
    public decimal Value { get; set; }
}
