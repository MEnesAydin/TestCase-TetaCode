using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestCase.Application.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public PagedData<T> Data { get; set; } = new();

    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; set; } = true;

    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; } = (int)HttpStatusCode.OK;

    [JsonPropertyName("errorMessages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? ErrorMessages { get; set; }

    [JsonConstructor]
    public PagedResult()
    {
        Data = new PagedData<T>();
    }

    public PagedResult(List<T> items)
    {
        Data = new PagedData<T> { Items = items };
    }

    public PagedResult(int statusCode, string errorMessage)
    {
        IsSuccessful = false;
        StatusCode = statusCode;
        ErrorMessages = new List<string> { errorMessage };
        Data = new PagedData<T>();
    }

    public PagedResult(int statusCode, List<string> errorMessages)
    {
        IsSuccessful = false;
        StatusCode = statusCode;
        ErrorMessages = errorMessages;
        Data = new PagedData<T>();
    }

    public void SetPaginationMetadata(int totalCount, int totalPages, int currentPage, int pageSize)
    {
        Data.Metadata ??= new();
        var paginationMetadata = new Dictionary<string, object>
        {
            { "totalCount", totalCount },
            { "totalPages", totalPages },
            { "currentPage", currentPage },
            { "pageSize", pageSize },
            { "hasPrevious", currentPage > 1 },
            { "hasNext", currentPage < totalPages }
        };
        Data.Metadata["pagination"] = paginationMetadata;
    }

    public void AddMetadata(string key, object value)
    {
        Data.Metadata ??= new();
        Data.Metadata[key] = value;
    }

    // Static factory methods
    public static PagedResult<T> Success(List<T> items, int totalCount, int currentPage, int pageSize)
    {
        var result = new PagedResult<T>(items)
        {
            IsSuccessful = true,
            StatusCode = (int)HttpStatusCode.OK
        };
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        result.SetPaginationMetadata(totalCount, totalPages, currentPage, pageSize);
        return result;
    }

    public static PagedResult<T> Failure(int statusCode, string errorMessage)
    {
        return new PagedResult<T>(statusCode, errorMessage);
    }

    public static PagedResult<T> Failure(int statusCode, List<string> errorMessages)
    {
        return new PagedResult<T>(statusCode, errorMessages);
    }

    public static PagedResult<T> Failure(string errorMessage)
    {
        return new PagedResult<T>((int)HttpStatusCode.InternalServerError, errorMessage);
    }

    public static PagedResult<T> Failure(List<string> errorMessages)
    {
        return new PagedResult<T>((int)HttpStatusCode.InternalServerError, errorMessages);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class PagedData<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();

    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Metadata { get; set; }

    public PagedData()
    {
        Items = new List<T>();
        Metadata = new Dictionary<string, object>();
    }
}