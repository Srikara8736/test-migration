using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Master;

public class CodeMasterResponseDto :CodeMasterDto
{
    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public Guid Id { get; set; }
}
