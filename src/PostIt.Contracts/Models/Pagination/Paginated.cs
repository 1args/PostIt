namespace PostIt.Contracts.Models.Pagination;

/// <summary>
/// Represents a paginated response with metadata about pagination state.
/// </summary>
/// <typeparam name="TData">The type of the items in the collection.</typeparam>
public class Paginated<TData>
{
    /// <summary>Parameters used to request this page (page number and page size).</summary>
    public PaginationParams PaginationParams { get; set; } = null!;
    
    /// <summary>Indicates whether a previous page exists.</summary>
    public bool HasPreviousPage { get; init; }
    
    /// <summary>Indicates whether a next page exists.</summary>
    public bool HasNextPage { get; init; }
    
    /// <summary>Total number of pages available.</summary>
    public int TotalPages { get; init; }

    /// <summary>Items contained in the current page.</summary>
    public IReadOnlyCollection<TData> Items { get; init; } = [];
}