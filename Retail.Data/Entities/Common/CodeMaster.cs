using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Common;

/// <summary>
/// Represents a CodeMaster
/// </summary>
[Table("CodeMaster")]
public class CodeMaster : BaseEntity
{
    /// <summary>
    /// Gets or sets the Type
    /// </summary>
    [StringLength(256)]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the Value
    /// </summary>
    [StringLength(256)]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the Order
    /// </summary>
    public int Order { get; set; }
}
