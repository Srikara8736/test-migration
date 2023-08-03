using Retail.Data.Entities.Common;
using Retail.Data.Entities.FileSystem;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a StoreData
/// </summary>
[Table("StoreData")]
public class StoreData : BaseEntity
{
    /// <summary>
    /// Gets or sets the VersionNumber
    /// </summary>
    public int VersionNumber { get; set; }


    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>
    public DateTime CreatedOn { get; set; }


    /// <summary>
    /// Gets or sets the UpdatedOn
    /// </summary>
    public DateTime? UpdatedOn { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }



    /// <summary>
    /// Gets or sets the reference of Status entity
    /// </summary>
    [ForeignKey(nameof(CodeMaster))]
    public Guid StatusId { get; set; }
    public virtual CodeMaster Status { get; set; }


    /// <summary>
    /// Gets or sets the reference of Document entity
    /// </summary>
    [ForeignKey(nameof(Document))]
    public Guid DocumentId { get; set; }
    public virtual Document Document { get; set; }

}
