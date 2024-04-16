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

    public decimal TotalPercentage { get; set; }

}


public class DaparmentListDto
{
    public decimal TotalArea { get; set; }
    public decimal TotalAreaIndoor { get; set; }
    public decimal TotalAreaOutdoor { get; set; }
    public decimal StoreArea { get; set; }
    public decimal StoreAreaIndoor { get; set; }
    public decimal StoreAreaOutdoor { get; set; }


    public List<ChartGridDto> chartGrids { get; set; } = new();
}
public class CustomerStoresDto
{
    public List<StoreDataDto> StoreData { get; set; } = new();
    public List<CoulmnListDto> ColumnList { get; set; } = new();

}



public class StoreDataDto
{
    public Guid StoreId { get; set; }
    public string StoreNumber { get; set; }
    public string StoreName { get; set; }  

    public List<CoulmnDataDto> CoulmnData { get; set; } = new();

}


public class CoulmnDataDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
}


public class CoulmnListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsParent { get; set; }
}





