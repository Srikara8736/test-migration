using Retail.DTOs.Cad;
using Retail.DTOs.Customers;

namespace Retail.DTOs.Stores;

public class StoreResponseDto : StoreDto
{
    /// <summary>
    /// Gets or sets the User ID
    /// </summary>
    public Guid Id { get; set; }


    public string StoreStatus { get; set; }
    public string PdfLink { get; set; }

    /// <summary>
    /// Gets or sets the Address
    /// </summary>
    public CustomerDto customer { get; set; }



    public List<ImageDto> StoreImages { get; set; } = new();

    public CadUploadHistoryResponseDto cadUploadHistory { get; set; }

    public List<StoreDataVersion> storeDataVersions { get; set; } = new();
    public List<StoreDataHistoryDto> StoreDataHistories { get; set; } = new();

}

public class StoreDataVersion
{
    public Guid Id { get; set; }
    public string Version { get; set; }
    public string VersionNumber { get; set; }
}


public class ImageDto
{
    public Guid Id { get; set; }
    public Guid ImageId { get; set; }
    public string ImageUrl { get; set; }

    public List<string> ThumnailUrls { get; set; } = new();
}


