namespace Retail.DTOs.Stores;

public class ChartGraphDto
{
    //public Guid? CategoryId { get; set; }
   // public Guid? ParentCategoryId { get; set; }
    public string ChartTitle { get; set; }
    public string ChartCategory { get; set; }
    public string ChartType { get; set; }
    public List<ChartItemDto> chartItems { get; set; } = new();

   
}

public class ChartItemDto
{
    public Guid? CategoryId { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string Key { get; set; }
    public decimal Value { get; set; }
    public string Unit { get; set; }
    public decimal TotalPercentage { get; set; }
}


public class ComparisionChartGraphDto
{
    public string ChartTitle { get; set; }
    public string ChartCategory { get; set; }
    public string ChartType { get; set; }
    public List<ComparisionChartItemDto> chartItems { get; set; } = new();


}

public class ComparisionChartItemDto
{
    public string Key { get; set; }
    public decimal V1Value { get; set; }
    public decimal V2Value { get; set; }
    public string Unit { get; set; }
    public decimal V1TotalPercentage { get; set; }
    public decimal V2TotalPercentage { get; set; }
}




public class StoreChartGraphDto
{
    public string ChartTitle { get; set; }
    public string ChartCategory { get; set; }
    public string ChartType { get; set; }
    public List<StoreChartItemDto> chartItems { get; set; } = new();


}

public class StoreChartItemDto
{
    public Guid? CategoryId { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string Key { get; set; }
    public decimal Value { get; set; }
    public string Unit { get; set; }
    public decimal TotalPercentage { get; set; }

    public List<StoreChartItemDto> chartItems { get; set; } = new();
}