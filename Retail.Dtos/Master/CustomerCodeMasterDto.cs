using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Master;

public class CustomerCodeMasterDto
{
    /// <summary>
    /// Gets or sets the StatusId
    /// </summary>
    [Required]
    public Guid StatusId { get; set; }


    /// <summary>
    /// Gets or sets the Order
    /// </summary>
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    /// <summary>
    /// Gets or sets the Value
    /// </summary>
    [StringLength(256)]
    public string StatusName { get; set; }

}
