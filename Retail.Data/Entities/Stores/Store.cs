using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.UserAccount;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.Data.Entities.Stores;


/// <summary>
/// Represents a Store
/// </summary>
[Table("Store")]
public class Store : BaseEntity
{
    /// <summary>
    /// Gets or sets the Store Name
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Store Number
    /// </summary>
    [Required]
    [StringLength(50)]
    public string StoreNumber { get; set; }


    /// <summary>
    /// Gets or sets the TotalArea
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalArea { get; set; }


    /// <summary>
    /// Gets or sets the reference of Status entity
    /// </summary>
    [ForeignKey(nameof(CodeMaster))]
    public Guid StatusId { get; set; }
    public virtual CodeMaster Status { get; set; }


    /// <summary>
    /// Gets or sets the reference of Customer entity
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }


    /// <summary>
    /// Gets or sets the reference of Address entity
    /// </summary>
    [ForeignKey(nameof(Address))]
    public Guid AddressId { get; set; }
    public virtual Address Address { get; set; }
}
