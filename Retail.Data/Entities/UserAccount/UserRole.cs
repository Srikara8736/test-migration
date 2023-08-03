using Retail.Data.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Retail.Data.Entities.UserAccount;

/// <summary>
/// Represents a UserRole
/// </summary>
[Table("UserRole")]
public class UserRole : BaseEntity
{

    /// <summary>
    /// Gets or sets the reference of Role entity
    /// </summary>
    [ForeignKey(nameof(Role))]
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; }


    /// <summary>
    /// Gets or sets the reference of User entity
    /// </summary>
    [ForeignKey(nameof(User))]
    public Guid? UserId { get; set; }
    public virtual User User { get; set; }
}
