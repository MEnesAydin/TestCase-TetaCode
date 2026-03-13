namespace TestCase.Domain.Abstractions;

public abstract class BaseQueryParameters
{
    private const int MaxPageSize = 100;
    private int? _pageSize = 10;

    public int? PageNumber { get; set; } = 1;

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public int GetPageNumber() => PageNumber ?? 1;

    public int GetPageSize() => PageSize ?? 10;

    //public string? SortBy { get; set; }

    //public bool? SortAscending { get; set; } = true;

    public string? SearchTerm { get; set; }

    public bool? IsDeleted { get; set; } = false;

}