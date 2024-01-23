using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Master;

public class CodeMasterDto
{
    /// <summary>
    /// Gets or sets the Type
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Type { get; set; }

    [Required]
    /// <summary>
    /// Gets or sets the Value
    /// </summary>
    [StringLength(256)]
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the Order
    /// </summary>
    public int Order { get; set; }
}
