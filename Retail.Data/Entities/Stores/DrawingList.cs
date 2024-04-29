using Retail.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a DrawingList
/// </summary>
[Table("DrawingList")]
public class DrawingList : BaseEntity
{
    /// <summary>
    /// Gets or sets the Drawing List Name
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Name { get; set; }


    /// <summary>
    /// Gets or sets the Drawing List Id
    /// </summary>
    [Required]
    [StringLength(256)]
    public string DrawingListId { get; set; }

    /// <summary>
    /// Gets or sets the StartDate
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the Date
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the Rev
    /// </summary>
    [StringLength(256)]
    public string? Rev { get; set; }

    /// <summary>
    /// Gets or sets the No
    /// </summary>
     [StringLength(255)]
    public string? No { get; set; }

    /// <summary>
    /// Gets or sets the Sign
    /// </summary>
    [StringLength(256)]
    public string? Sign { get; set; }

    /// <summary>
    /// Gets or sets the Note
    /// </summary>    
    [StringLength(256)]
    public string? Note { get; set; }


    /// <summary>
    /// Gets or sets the Status
    /// </summary>    
    [StringLength(256)]
    public string? Status { get; set; }



    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(StoreData))]
    public Guid StoreDataId { get; set; }
    public virtual StoreData StoreData { get; set; }



}
