using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Stores;

public class DeleteImageDto
{

    /// <summary>
    /// Store Id / Customer Id
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Image Mid Table Id
    /// </summary>
    [Required]
    public Guid ImageMidId { get; set; }

    /// <summary>
    /// Image Id
    /// </summary>
    [Required]
    public Guid ImageId { get; set; }

    /// <summary>
    /// ImgUrl
    /// </summary>
    [Required]
    public string ImgUrl { get;  set; }

    /// <summary>
    /// Should be either Store / Customer
    /// </summary>
    [Required]
    public string Type { get; set; }
}
