namespace PostIt.Contracts.Models.Sorting;

/// <summary>
/// Represents sorting parameters for a query.
/// </summary>
public class SortParams
{
    /// <summary>Represents sorting parameters for a query.</summary>
    public string Criteria { get; set; } = string.Empty;
    
    /// <summary>Indicates whether the sort order is ascending.</summary>
    public bool IsAscending { get; set; } = true;
}