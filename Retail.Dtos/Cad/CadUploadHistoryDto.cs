using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.Cad;

public class CadUploadHistoryDto
{
    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the FileName
    /// </summary>
    public string UploadId { get; set; }

    /// <summary>
    /// Gets or sets the UploadOn
    /// </summary>
    public DateTime UploadOn { get; set; }


    /// <summary>
    /// Gets or sets the CreatedOn
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the Status
    /// </summary>
    public bool Status { get; set; }


    /// <summary>
    /// Gets or sets the reference of Store entity
    /// </summary>
    public Guid StoreId { get; set; }
}
