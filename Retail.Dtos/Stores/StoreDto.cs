using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Retail.DTOs.Customers;

namespace Retail.DTOs.Stores;

public class StoreDto
{
    /// <summary>
    /// Gets or sets the Store Name
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Store Number
    /// </summary>
    [Required]
    [StringLength(50)]
    public string StoreNumber { get; set; }


    /// <summary>
    /// Gets or sets the TotalArea
    /// </summary>
    public decimal? TotalArea { get; set; }


    /// <summary>
    /// Gets or sets the TotalArea
    /// </summary>
    public decimal? IndoorSalesArea { get; set; }

    /// <summary>
    /// Gets or sets the CustomerId
    /// </summary>
    public Guid CustomerId { get; set; }


    /// <summary>
    /// Gets or sets the AddressId
    /// </summary>
    public Guid? AddressId { get; set; }

    /// <summary>
    /// Gets or sets the Address
    /// </summary>
    public AddressDto Address { get; set; }

    /// <summary>
    /// Gets or sets the StatusId
    /// </summary>
    public Guid StatusId { get; set; }
}

public class CustomerStoreDto
{

    public int NumberOfStore { get; set; }
    public decimal? TotalStoreArea { get; set; }
    public decimal TotalSalesArea { get; set; }
    public List<StoreResponseDto> Store { get; set; } = new();
    public List<StoreStatusDto> StoreStatus { get; set; } = new();

}

public class StoreStatusDto
{
    public string Status { get; set; }
    public int NumberOfStore { get; set; }    
    public string? Property { get; set; }    
}