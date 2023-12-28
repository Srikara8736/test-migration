using Retail.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a StoreSpace
/// </summary>
[Table("StoreSpace")]
public class StoreSpace : BaseEntity
{
    /// <summary>
    /// Gets or sets the Unit Name
    /// </summary>
    [Required]
    [StringLength(10)]
    public string Unit { get; set; }


    /// <summary>
    /// Gets or sets the Area
    /// </summary>
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Area { get; set; }


    /// <summary>
    /// Gets or sets the Pieces
    /// </summary>
  
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Pieces { get; set; }


    /// <summary>
    /// Gets or sets the Articles
    /// </summary>
   
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Articles { get; set; }

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
    /// Gets or sets the reference of Space entity
    /// </summary>
    [ForeignKey(nameof(Space))]
    public Guid? SpaceId { get; set; }
    public virtual Space Space { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store Data entity
    /// </summary>
    [ForeignKey(nameof(StoreData))]
    public Guid StoreDataId { get; set; }
    public virtual StoreData StoreData { get; set; }


    /// <summary>
    /// Gets or sets the reference of Code Master entity
    /// </summary>
    [ForeignKey(nameof(CadFileType))]
    public Guid CadFileTypeId { get; set; }
    public virtual CodeMaster CadFileType { get; set; }
}
