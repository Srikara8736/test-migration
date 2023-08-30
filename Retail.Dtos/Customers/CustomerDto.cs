using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Customers;


public class CustomerDto
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
    /// Gets or sets the Phone
    /// </summary>

    [Required]
    public string CreatedBy { get; set; }


    /// <summary>
    /// Gets or sets the Phone
    /// </summary>
    public string? UpdatedBy { get; set; }



  
    public Guid? AddressId { get; set; }

    /// <summary>
    /// Gets or sets the reference of Address 
    /// </summary> 
    public AddressDto Address { get; set; }


    public Guid? LogoImageId { get; set; }

    public IFormFile? CustomerLogo { get; set; }

    /// <summary>
    /// Gets or sets the reference of Image entity
    /// </summary>
    public Guid? BackgroundImageId { get; set; }

}

public class AddressDto 
{

    /// <summary>
    /// Gets or sets the Street
    /// </summary>
    [StringLength(256)]
    public string Street { get; set; }

    /// <summary>
    /// Gets or sets the City
    /// </summary>
    [StringLength(256)]
    public string City { get; set; }

    /// <summary>
    /// Gets or sets the Country
    /// </summary>
    [StringLength(256)]
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the ZipCode
    /// </summary>
    [StringLength(10)]
    public string ZipCode { get; set; }

    /// <summary>
    /// Gets or sets the Latitude
    /// </summary>
    [StringLength(50)]
    public string? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the Longitude
    /// </summary>
    [StringLength(50)]
    public string? Longitude { get; set; }
}