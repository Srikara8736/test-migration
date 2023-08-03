using Retail.Data.Entities.FileSystem;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a StoreImage
/// </summary>
[Table("StoreImage")]
public class StoreImage : BaseEntity
{


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of Image entity
    /// </summary>
    [ForeignKey(nameof(Image))]
    public Guid ImageId { get; set; }
    public virtual Image Image { get; set; }

}
