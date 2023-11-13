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
    public string PasswordHash { get; set; }


    /// <summary>
    /// Gets or sets the PhoneNumber
    /// </summary>
    public string PhoneNumber { get; set; }


    /// <summary>
    /// Gets or sets the RoleId
    /// </summary>
    [Required]
    public Guid RoleId { get; set; }


    [Required]
    public Guid StatusId { get; set; }

    /// <summary>
    /// Gets or sets the CustomerId
    /// </summary>
    public Guid? CustomerId { get; set; }


}

