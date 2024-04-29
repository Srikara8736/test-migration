using System.ComponentModel.DataAnnotations;

namespace Retail.DTOs.Stores;

public class DrawingListDto
{
    /// <summary>
    /// Gets or sets the Drawing List Name
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Gets or sets the Drawing List Id
    /// </summary> 
    public string Pno { get; set; }

    /// <summary>
    /// Gets or sets the StartDate
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the Date
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the Rev
    /// </summary>
    public string? Rev { get; set; }

    /// <summary>
    /// Gets or sets the No
    /// </summary>
    public string? No { get; set; }

    /// <summary>
    /// Gets or sets the Sign
    /// </summary>
    public string? Sign { get; set; }

    /// <summary>
    /// Gets or sets the Note
    /// </summary> 
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets Status
    /// </summary>
    public string Status { get; set; }

    public Guid StoreId { get; set; }

    public Guid StoreDataId { get; set; }
    public string? PdfLink { get; set; }
}
