namespace RetailApp.Helpers;

public class CustomerParam : PagingParam
{
    /// <summary>
    /// Customer identifier 
    /// </summary>
    public Guid? CustomerId { get; set; }
}
