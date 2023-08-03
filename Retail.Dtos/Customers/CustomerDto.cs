using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Customers;


public class CustomerDto
{
    /// <summary>
    /// Gets or sets the CustomerNo
    /// </summary>
    [Required]
    [StringLength(5)]
    public string CustomerNo { get; set; }

    /// <summary>
    /// Gets or sets the CustomerName
    /// </summary>
    [Required]
    public string CustomerName { get; set; }


    /// <summary>
    /// Gets or sets the CustomerCode
    /// </summary>
    [Required]
    [StringLength(5)]
    public string CustomerCode { get; set; }

    /// <summary>
    /// Gets or sets the Mail
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the Phone
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the CompanyName
    /// </summary>
    [StringLength(50)]
    public string? CompanyName { get; set; }


}
