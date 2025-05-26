using Microsoft.EntityFrameworkCore;
using PostIt.Contracts.Models.Pagination;

namespace PostIt.Common.Extensions;

/// <summary>
/// Provides extension methods for paginating queryable data.
/// </summary>
public static class PaginationExtension
{
    /// <summary>
    /// Paginates the query according to the specified parameters.
    /// </summary>
    /// <typeparam name="TData">Type of the data being paginated.</typeparam>
    /// <param name="query">Source query to paginate.</param>
    /// <param name="paginationParams">Pagination parameters (page number and page size).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><see cref="Paginated{TData}"/> object containing the page data and metadata.</returns>
    public static async Task<Paginated<TData>> AsPaginatedAsync<TData>(
        this IQueryable<TData> query, 
        PaginationParams paginationParams,
        CancellationToken cancellationToken)
    {
        var result = await query
            .Skip(paginationParams.PageSize * paginationParams.PageNumber)
            .Take(paginationParams.PageSize)
            .ToListAsync(cancellationToken);
        
        var totalCount = query.LongCount();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationParams.PageSize);
        
        return new Paginated<TData>
        {
            Items = result,
            PaginationParams = paginationParams,
            TotalPages = totalPages,
            HasPreviousPage = paginationParams.PageNumber > PaginationParams.DefaultPageNumber,
            HasNextPage = paginationParams.PageNumber < totalPages
        };
    }
}