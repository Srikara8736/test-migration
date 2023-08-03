using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a CategoryAreaTypeGroup
/// </summary>
[Table("StoreCategoryAreaTypeGroup")]
public class StoreCategoryAreaTypeGroup : BaseEntity
{

    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// Gets or sets the reference of Category entity
    /// </summary>
    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }


    /// Gets or sets the reference of Category entity
    /// </summary>
    [ForeignKey(nameof(AreaTypeGroup))]
    public Guid AreaTypeGroupId { get; set; }
    public virtual AreaTypeGroup AreaTypeGroup { get; set; }


    /// Gets or sets the reference of Category entity
    /// </summary>
    [ForeignKey(nameof(Space))]
    public Guid? SpaceId { get; set; }
    public virtual Space Space { get; set; }


}
