using System.ComponentModel.DataAnnotations;

namespace Retail.DTOs.Roles;

public class RoleDto
{
    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    [StringLength(255)]
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
