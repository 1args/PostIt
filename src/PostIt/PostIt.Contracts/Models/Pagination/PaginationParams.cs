namespace PostIt.Contracts.Models.Pagination;

/// <summary>
/// Represents parameters for pagination (page number and page size).
/// </summary>
public class PaginationParams
{
    /// <summary>Default page number (starts from 0).</summary>
    public const int DefaultPageNumber = 0;
    
    /// <summary> Maximum allowed page size.</summary>
    private const int MaxPageSize = 25;

    /// <summary>Current page number (zero-based).</summary>
    public int PageNumber { get; init; } = DefaultPageNumber;

    /// <summary>Number of items per page.</summary>
    public int PageSize { get; init; } = MaxPageSize;
}