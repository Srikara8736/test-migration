using System.ComponentModel.DataAnnotations;

namespace Retail.DTOs.Stores;

public class DrawingListResponseDto : DrawingListDto
{
    /// <summary>
    /// Gets or sets the User ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the User ID
    /// </summary>
    public string StoreStatus { get; set; }
}
