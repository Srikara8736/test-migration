using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs;

/// <summary>
/// Represents a PaginationResultDto
/// </summary>
public class PaginationResultDto<T> : ResultDto<T>
{
    /// <summary>
    /// Gets or sets the Total Count
    /// </summary>
    public int? TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the Total Pages
    /// </summary>
    public int? TotalPages { get; set; }

}

