namespace PostIt.Contracts.Models.Sorting;

/// <summary>
/// Represents sorting parameters for a query.
/// </summary>
public class SortParams
{
    /// <summary>Represents sorting parameters for a query.</summary>
    public string Criteria { get; init; } = string.Empty;
    
    /// <summary>Indicates whether the sort order is ascending.</summary>
    public bool IsAscending { get; init; } = true;
}