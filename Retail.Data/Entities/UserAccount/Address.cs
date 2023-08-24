using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Data.Entities.UserAccount;

/// <summary>
/// Represents a Address
/// </summary>
[Table("Address")]
public class Address : BaseEntity
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
    public string? Latitude   { get; set; }

    /// <summary>
    /// Gets or sets the Longitude
    /// </summary>
    [StringLength(50)]
    public string? Longitude { get; set; }
}