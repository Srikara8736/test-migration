using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.FileSystem;

/// <summary>
/// Represents a Image
/// </summary>
[Table("Image")]
public class Image : BaseEntity
{

    /// <summary>
    /// Gets or sets the File Name
    /// </summary>
    [Required]
    [StringLength(256)]
    public string FileName { get; set; }


    /// <summary>
    /// Gets or sets the FileType
    /// </summary>
    [StringLength(256)]
    public string FileType { get; set; }


    /// <summary>
    /// Gets or sets the FileExtension
    /// </summary>
    [StringLength(256)]
    public string FileExtension { get; set; }


    /// <summary>
    /// Gets or sets the UploadedOn
    /// </summary>
    public DateTime UploadedOn { get; set; }


    /// <summary>
    /// Gets or sets the UploadedBy
    /// </summary>
    public string UploadedBy { get; set; }
}
