using Retail.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a OrderList
/// </summary>
[Table("OrderList")]
public class OrderList : BaseEntity
{
    /// <summary>
    /// Gets or sets the ArticleNumber
    /// </summary>
 
    [StringLength(256)]
    public string ArticleNumber { get; set; }

    /// <summary>
    /// Gets or sets the Order List Name
    /// </summary>

    [StringLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Quantity
    /// </summary>
    public int Quantity  { get; set; }

    /// <summary>
    /// Gets or sets the CadData
    /// </summary>
    public string? CadData { get; set; }

    /// <summary>
    /// Gets or sets the Producer
    /// </summary>
    [StringLength(256)]
    public string Producer     { get; set; }

    /// <summary>
    /// Gets or sets the No
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the Sign
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Sum { get; set; }

    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(PackageData))]
    public Guid PackageDataId { get; set; }
    public virtual PackageData PackageData { get; set; }

}
