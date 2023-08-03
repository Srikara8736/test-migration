using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.UserAccounts;


public class UserDto
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
    /// Gets or sets the UserName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the Email
    /// </summary>
    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the Password
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the IsActive
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the RoleId
    /// </summary>
    [Required]
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets the CustomerId
    /// </summary>
    public Guid? CustomerId { get; set; }


}

