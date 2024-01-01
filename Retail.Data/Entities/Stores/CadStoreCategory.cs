using Retail.Data.Entities.Cad;
using Retail.Data.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a CadStoreCategory
/// </summary>
[Table("CadStoreCategory")]
public class CadStoreCategory : BaseEntity
{

    /// <summary>
    /// Gets or sets the Date
    /// </summary>
    public DateTime CreatedDate { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(UploadHistory))]
    public Guid UploadHistoryId { get; set; }
    public virtual CadUploadHistory UploadHistory { get; set; }



    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }


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


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Status))]
    public Guid CadTypeId { get; set; }
    public virtual CodeMaster Status { get; set; }
}
