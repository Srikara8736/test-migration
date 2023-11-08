using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Stores;

public class StoreComparisionRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required]
    public Guid Version1 { get; set; }

    [Required]
    public Guid Version2 { get; set; }
}
