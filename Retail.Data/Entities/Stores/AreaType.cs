using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a AreaType
/// </summary>
[Table("AreaType")]
public class AreaType : BaseEntity
{
    /// <summary>
    /// Gets or sets the Area Type Name
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Name { get; set; }


}
