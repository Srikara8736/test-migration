using Retail.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a DrawingList
/// </summary>
[Table("GeneralListTypeData")]
public class GeneralListTypeData : BaseEntity
{
    /// <summary>
    /// Gets or sets the Koncept1s
    /// </summary>
    public string? Koncept1 { get; set; }


    /// <summary>
    /// Gets or sets the Koncept2
    /// </summary>
    public string? Koncept2 { get; set; }


    /// <summary>
    /// Gets or sets the Realm2
    /// </summary>

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Realm2 { get; set; }


    /// <summary>
    /// Gets or sets the RealPercentage
    /// </summary>
    public string? RealPercentage { get; set; }



    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }

}
