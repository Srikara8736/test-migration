using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Retail.Data.Entities.UserAccount;

namespace Retail.Data.Entities.Customers;

/// <summary>
/// Represents a Customer
/// </summary>
[Table("Customer")]
public class Customer : BaseEntity
{

    /// <summary>
    /// Gets or sets the CustomerName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Mail
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the Phone
    /// </summary>
    [Required]
    [StringLength(50)]
    public string PhoneNumber { get; set; }


    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>

    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the CreatedBy
    /// </summary>
    [StringLength(256)]
    public string CreatedBy { get; set; }


    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>

    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// Gets or sets the CreatedBy
    /// </summary>
    [StringLength(256)]
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the reference of Address entity
    /// </summary>
    [ForeignKey(nameof(Address))]
    public Guid AddressId { get; set; }
    public virtual Address Address { get; set; }

    /// <summary>
    /// Gets or sets the IsDeleted
    /// </summary>
    [Required]
    public bool IsDeleted { get; set; }

}
