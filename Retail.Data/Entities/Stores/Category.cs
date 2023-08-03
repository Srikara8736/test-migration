using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a Category
/// </summary>
[Table("Category")]
public class Category : BaseEntity
{
    /// <summary>
    /// Gets or sets the Store Name
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Name { get; set; }


    /// <summary>
    /// Gets or sets the CAD service Number
    /// </summary>
    [Required]
    public int CadServiceNumber { get; set; }



    /// <summary>
    /// Gets or sets the reference of AreaType entity
    /// </summary>
    [ForeignKey(nameof(AreaType))]
    public Guid AreaTypeId { get; set; }
    public virtual AreaType AreaType { get; set; }
}
