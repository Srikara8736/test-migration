using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a Space
/// </summary>
[Table("Space")]
public class Space : BaseEntity
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
    /// Gets or sets the reference of Category entity
    /// </summary>
    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
}
