using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Cad;

public class CadUploadDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required]
    public IFormFile CadFile { get; set; }

    public string? Type { get; set; }
}
