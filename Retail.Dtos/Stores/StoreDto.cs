using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalArea { get; set; }

    /// <summary>
    /// Gets or sets the CustomerId
    /// </summary>
    public Guid CustomerId { get; set; }


    /// <summary>
    /// Gets or sets the TotalArea
    /// </summary>
    public Guid AddressId { get; set; }
}
