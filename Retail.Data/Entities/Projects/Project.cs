using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Projects;

/// <summary>
/// Represents a Project
/// </summary>
[Table("Project")]
public class Project : BaseEntity
{
    /// <summary>
    /// Gets or sets the Project Name
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Name { get; set; }


    /// <summary>
    /// Gets or sets the Description
    /// </summary>
    public string? Description { get; set; }


    /// <summary>
    /// Gets or sets the StartDate
    /// </summary>
    public DateTime StartDate { get; set; }


    /// <summary>
    /// Gets or sets the EndDate
    /// </summary>
    public DateTime EndDate { get; set; }


}
