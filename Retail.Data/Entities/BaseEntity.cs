using System.ComponentModel.DataAnnotations;

namespace Retail.Data.Entities;

public class BaseEntity
{
    /// <summary>
    /// Gets or sets the id (primary key)
    /// </summary>
    [Key]
    public Guid Id { get; set; }
}
