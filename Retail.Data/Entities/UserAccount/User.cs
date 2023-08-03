using Retail.Data.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Retail.Data.Entities.Common;

namespace Retail.Data.Entities.UserAccount;

/// <summary>
/// Represents a User
/// </summary>
[Table("User")]
public class User : BaseEntity
{
    /// <summary>
    /// Gets or sets the FirstName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the LastName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string LastName { get; set; }


    /// <summary>
    /// Gets or sets the Email
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the Password
    /// </summary>
    [Required]
    public string PasswordHash { get; set; }





    /// <summary>
    /// Gets or sets the IsDeleted
    /// </summary>
    [Required]
    public bool IsDeleted { get; set; }


    /// <summary>
    /// Gets or sets the PhoneNumber
    /// </summary>
    [StringLength(50)]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>
    public DateTime CreatedOn { get; set; }


    /// <summary>
    /// Gets or sets the UpdatedOn
    /// </summary>
    public DateTime? UpdatedOn { get; set; }


    /// <summary>
    /// Gets or sets the EmailConfirmed
    /// </summary>
    [Required]
    public bool EmailConfirmed { get; set; }


    /// <summary>
    /// Gets or sets the PhoneNumberConfirmed
    /// </summary>
    [Required]
    public bool PhoneNumberConfirmed { get; set; }



    /// <summary>
    /// Gets or sets the reference of Status entity
    /// </summary>
    [ForeignKey(nameof(CodeMaster))]
    public Guid StatusId { get; set; }
    public virtual CodeMaster Status { get; set; }



    /// <summary>
    /// Gets or sets the reference of Customer entity
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public Guid? CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}
