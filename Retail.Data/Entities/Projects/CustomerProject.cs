using Retail.Data.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Projects;



/// <summary>
/// Represents a CustomerProject
/// </summary>
[Table("CustomerProject")]
public class CustomerProject : BaseEntity
{

    /// <summary>
    /// Gets or sets the reference of Customer entity
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }


    /// <summary>
    /// Gets or sets the reference of Project entity
    /// </summary>
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; }

}
