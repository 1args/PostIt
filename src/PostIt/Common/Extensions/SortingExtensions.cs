using System.Linq.Dynamic;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Models.Sorting;

namespace PostIt.Common.Extensions;

/// <summary>
/// Provides extension methods for applying dynamic sorting to queryable data.
/// </summary>
public static class SortingExtensions
{
    /// <summary>
    /// Applies dynamic sorting to the query.
    /// </summary>
    /// <typeparam name="TData">Type of data in the query.</typeparam>
    /// <param name="query">Query to sort.</param>
    /// <param name="sortParams">Sorting criteria and direction.</param>
    /// <returns>Sorted <see cref="IQueryable{T}"/>.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the provided sorting criteria is invalid or unsupported.
    /// </exception>
    public static IQueryable<TData> SortedBy<TData>(
        this IQueryable<TData> query,
        SortParams? sortParams)
    {
        if (sortParams is null)
        {
            return query.Order();
        }

        var expression = $"{sortParams.Criteria} {(sortParams.IsAscending ? "ascending" : "descending")}";
        
        try
        {
            return query.OrderBy(expression);
        }
        catch (Exception)
        {
            throw new NotFoundException(
                $"Failed to sort by criterion '{sortParams.Criteria}'. " +
                $"Please refer to the documentation for the eligibility criteria for the relevant requests.");
        }
    }
}