using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.DTOs.Stores;

public class GeneralListTypeDataDto
{

    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Koncept1s
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the Realm2
    /// </summary>

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Realm2 { get; set; }


    /// <summary>
    /// Gets or sets the RealPercentage
    /// </summary>
    public string? RealPercentage { get; set; }


    public Guid StoreId { get; set; }

    public string StoreName { get; set; }

}
