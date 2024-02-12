using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retail.Data.Entities.Stores;

namespace Retail.Data.Entities.Cad;

/// <summary>
/// Represents a CadUploadHistory
/// </summary>
[Table("CadUploadHistory")]
public class CadUploadHistory : BaseEntity
{

    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    [StringLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the FileName
    /// </summary>
    [StringLength(256)]
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
    [ForeignKey(nameof(Store))]
    public Guid StoreId { get; set; }
    public virtual Store Store { get; set; }


    /// <summary>
    /// Gets or sets the reference of StoreData entity
    /// </summary>
    [ForeignKey(nameof(StoreData))]
    public Guid? StoreDataId { get; set; }
    public virtual StoreData StoreData { get; set; }
}

