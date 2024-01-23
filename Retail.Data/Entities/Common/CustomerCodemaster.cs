using Retail.Data.Entities.Customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Common;

public class CustomerCodemaster : BaseEntity
{
    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    [Required]
    [StringLength(256)]
    public string StatusName { get; set; }


    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>
    [Required]
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the reference of Image entity
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }


    /// <summary>
    /// Gets or sets the reference of Image entity
    /// </summary>
    [ForeignKey(nameof(CodeMaster))]
    public Guid StatusId { get; set; }
    public virtual CodeMaster CodeMaster { get; set; }
}
