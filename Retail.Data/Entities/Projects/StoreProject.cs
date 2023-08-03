using Retail.Data.Entities.Stores;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Projects;



/// <summary>
/// Represents a StoreProject
/// </summary>
[Table("StoreProject")]
public class StoreProject : BaseEntity
{

    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of Project entity
    /// </summary>
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; }

}
