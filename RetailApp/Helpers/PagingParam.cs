using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RetailApp.Helpers;

/// <summary>
/// Represents a PagingParam
/// </summary>
public class PagingParam
{

    /// <summary>
    /// Search with Keyword
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    ///  Page number for the List
    /// </summary>
    [BindRequired]
    public int PageIndex { get; set; }

    /// <summary>
    /// Page Size for the page
    /// </summary>
    [BindRequired]
    public int PageSize { get; set; }
}

