using Retail.DTOs.Customers;

namespace Retail.DTOs.Stores;

public class StoreResponseDto : StoreDto
{
    /// <summary>
    /// Gets or sets the User ID
    /// </summary>
    public Guid Id { get; set; }


    public string StoreStatus { get; set; }

    /// <summary>
    /// Gets or sets the Address
    /// </summary>
    public CustomerDto customer { get; set; }

    /// <summary>
    /// Gets or sets the Address
    /// </summary>
    public AddressDto Address { get; set; }

    public List<string> StoreImages { get; set; } = new();

}

