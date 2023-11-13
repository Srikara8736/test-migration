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
    public Guid FirstStoreId { get; set; }

    [Required]
    public Guid FirstVersionId { get; set; }

    [Required]
    public Guid SecondStoreId { get; set; }

    [Required]
    public Guid SecondVersionId { get; set; }
}

