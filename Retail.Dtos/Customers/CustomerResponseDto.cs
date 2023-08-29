using Retail.DTOs.Stores;

namespace Retail.DTOs.Customers;

/// <summary>
/// Represents a Customer DTO Model
/// </summary>
public class CustomerResponseDto : CustomerDto
{
    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ImageUrl
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// Gets or sets the Customer Stores
    /// </summary>
    public CustomerStoreDto CustomerStores { get; set; }


    public List<ImageDto> CustomerImages { get; set; } = new();


}
