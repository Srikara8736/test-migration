using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Services.Common;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    private Task PagedListItem(IQueryable<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
        return Task.CompletedTask;
    }
    public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();

        var items = source;

        if (pageNumber > 0 && pageSize > 0)
            items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        // var items = source;

        var pageList = new PagedList<T>();

        await pageList.PagedListItem(items, count, pageNumber, pageSize);
        return pageList;
    }
}
