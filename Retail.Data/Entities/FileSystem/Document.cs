using Retail.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Data.Entities.FileSystem;

/// <summary>
/// Represents a Document
/// </summary>
[Table("Document")]
public class Document : BaseEntity
{

    /// <summary>
    /// Gets or sets the File Name
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }


    /// <summary>
    /// Gets or sets the Path
    /// </summary>
    public string Path { get; set; }


    /// <summary>
    /// Gets or sets the reference of Status entity
    /// </summary>
    [ForeignKey(nameof(CodeMaster))]
    public Guid StatusId { get; set; }
    public virtual CodeMaster Status { get; set; }
}
