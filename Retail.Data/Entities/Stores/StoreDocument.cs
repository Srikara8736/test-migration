using Retail.Data.Entities.FileSystem;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a Store Document
/// </summary>
[Table("StoreDocument")]
public class StoreDocument : BaseEntity
{
   
    /// <summary>
    /// Gets or sets the reference of Status entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of Document entity
    /// </summary>
    [ForeignKey(nameof(Document))]
    public Guid DocumentId { get; set; }
    public virtual Document Document { get; set; }

}
