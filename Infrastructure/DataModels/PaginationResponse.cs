namespace Infrastructure.DataModels;

public class PaginationResponse<TResponse>
{
    /// <summary>
    ///     Collection items with pagination
    /// </summary>
    public IReadOnlyCollection<TResponse> Items { get; }
    /// <summary>
    ///     Page number
    /// </summary>
    public int PageNumber { get; }
    /// <summary>
    ///     Total pages after pagination
    /// </summary>
    public int TotalPages { get; }
    /// <summary>
    ///     Count  all items after query
    /// </summary>
    public int TotalCount { get; }
    /// <summary>
    ///     page size
    /// </summary>
    public int PageSize { get; }
    /// <summary>
    ///     
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;  
    /// <summary>
    /// 
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginationResponse(IReadOnlyCollection<TResponse> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}