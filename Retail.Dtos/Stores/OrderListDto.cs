using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail.DTOs.Stores;

public class OrderListDto
{

    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ArticleNumber
    /// </summary>
    public string ArticleNumber { get; set; }

    /// <summary>
    /// Gets or sets the Order List Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the CadData
    /// </summary>
    public string? CadData { get; set; }

    /// <summary>
    /// Gets or sets the Producer
    /// </summary>
    [StringLength(256)]
    public string Producer { get; set; }

    /// <summary>
    /// Gets or sets the No
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the Sign
    /// </summary>
    public decimal Sum { get; set; }

    public Guid StoreId { get; set; }

}

public class PackageDataDto
{

    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the PackageName
    /// </summary>
    public string PackageName { get; set; }



    /// <summary>
    /// Gets or sets the CreatedBy
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the CreatedBy
    /// </summary>
    public DateTime? CreatedDate { get; set; }


    /// <summary>
    /// Gets or sets the FileName
    /// </summary>
    [Required]
    [StringLength(256)]
    public string FileName { get; set; }


    /// <summary>
    /// Gets or sets the Date
    /// </summary>
    [StringLength(256)]
    public DateTime? Date { get; set; }


    /// <summary>
    /// Gets or sets the Drawing
    /// </summary>
    [StringLength(256)]
    public string? Drawing { get; set; }


    /// <summary>
    /// Gets or sets the Comment
    /// </summary>
    [StringLength(256)]
    public string? Comment { get; set; }


    /// <summary>
    /// Gets or sets the Addon
    /// </summary>
    [StringLength(256)]
    public string? Addon { get; set; }


    /// <summary>
    /// Gets or sets the LibraryVersion
    /// </summary>
    [StringLength(256)]
    public string? LibraryVersion { get; set; }


    /// <summary>
    /// Gets or sets the EstabishmentConfiguration
    /// </summary>
    [StringLength(256)]
    public string? EstabishmentConfiguration { get; set; }



    /// <summary>
    /// Gets or sets the ProductList
    /// </summary>
    [StringLength(256)]
    public string? ProductList { get; set; }


    /// <summary>
    /// Gets or sets the  Store id
    /// </summary>
    public Guid StoreId { get; set; }


    /// <summary>
    /// Gets or sets the  Store name
    /// </summary>
    public string StoreName { get; set; }

    /// <summary>
    /// Gets or sets the Status
    /// </summary>
    public Guid StatusId { get; set; }

    public string StatusName { get; set; }



    public List<OrderListDto> OrderList { get; set; } = new List<OrderListDto>();

}