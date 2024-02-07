using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Master;

public class CustomerCodeMasterResponseDto 
{
    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public Guid Id { get; set; }
    public string StatusName { get; set; }
    public DateTime CreatedOn { get; set; }
}
