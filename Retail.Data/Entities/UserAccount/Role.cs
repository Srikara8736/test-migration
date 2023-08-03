using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Data.Entities.UserAccount;

/// <summary>
/// Represents a Role
/// </summary>
[Table("Role")]
public class Role : BaseEntity
{

    /// <summary>
    /// Gets or sets the title
    /// </summary>
    [StringLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the active
    /// </summary>
    public bool IsActive { get; set; }

}