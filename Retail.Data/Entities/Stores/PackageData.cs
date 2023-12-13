using Retail.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a PackageData
/// </summary>
[Table("PackageData")]
public class PackageData : BaseEntity
{
    /// <summary>
    /// Gets or sets the PackageName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string PackageName { get; set; }



    /// <summary>
    /// Gets or sets the CreatedBy
    /// </summary>
    [Required]
    [StringLength(256)]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the CreatedBy
    /// </summary>
    public DateTime? CreatedDate { get; set; }


    /// <summary>
    /// Gets or sets the FileName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string FileName { get; set; }


    /// <summary>
    /// Gets or sets the Date
    /// </summary>
    [StringLength(256)]
    public DateTime? Date { get; set; }


    /// <summary>
    /// Gets or sets the Drawing
    /// </summary>
    [StringLength(256)]
    public string? Drawing { get; set; }


    /// <summary>
    /// Gets or sets the Comment
    /// </summary>
    [StringLength(256)]
    public string? Comment { get; set; }


    /// <summary>
    /// Gets or sets the Addon
    /// </summary>
    [StringLength(256)]
    public string? Addon { get; set; }


    /// <summary>
    /// Gets or sets the LibraryVersion
    /// </summary>
    [StringLength(256)]
    public string? LibraryVersion { get; set; }


    /// <summary>
    /// Gets or sets the EstabishmentConfiguration
    /// </summary>
    [StringLength(256)]
    public string? EstabishmentConfiguration { get; set; }



    /// <summary>
    /// Gets or sets the ProductList
    /// </summary>
    [StringLength(256)]
    public string? ProductList { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Status))]
    public Guid StatusId { get; set; }
    public virtual CodeMaster Status { get; set; }
}
