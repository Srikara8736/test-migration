using Retail.Data.Entities.FileSystem;
using System.ComponentModel.DataAnnotations.Schema;
namespace Retail.Data.Entities.Customers;

/// <summary>
/// Represents a Customer Image
/// </summary>
[Table("CustomerImage")]
public class CustomerImage : BaseEntity
{

    /// <summary>
    /// Gets or sets the reference of Customer entity
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }


    /// <summary>
    /// Gets or sets the reference of Image entity
    /// </summary>
    [ForeignKey(nameof(Image))]
    public Guid ImageId { get; set; }
    public virtual Image Image { get; set; }

}
