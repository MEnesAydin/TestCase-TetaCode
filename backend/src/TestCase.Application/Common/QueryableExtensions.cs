using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestCase.Domain.Abstractions;

namespace TestCase.Application.Common;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source,
        BaseQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = parameters.GetPageNumber();
        var pageSize = parameters.GetPageSize();

        var totalCount = await source.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = new PagedResult<T>(items)
        {
            IsSuccessful = true,
            StatusCode = 200
        };
        
        result.SetPaginationMetadata(totalCount, totalPages, pageNumber, pageSize);

        return result;
    }
    
    public static async Task<PagedResult<TResult>> ToPagedResultAsync<T, TResult>(
        this IQueryable<T> source,
        BaseQueryParameters parameters,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = parameters.GetPageNumber();
        var pageSize = parameters.GetPageSize();

        var totalCount = await source.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        var result = new PagedResult<TResult>(items)
        {
            IsSuccessful = true,
            StatusCode = 200
        };
        
        result.SetPaginationMetadata(totalCount, totalPages, pageNumber, pageSize);

        return result;
    }
    
    public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> source,
        BaseQueryParameters parameters,
        Expression<Func<T, bool>> searchPredicate)
    {
        if (string.IsNullOrWhiteSpace(parameters.SearchTerm))
            return source;

        return source.Where(searchPredicate);
    }
    
    public static IQueryable<T> ApplyFilters<T>(
        this IQueryable<T> source,
        BaseQueryParameters parameters,
        Expression<Func<T, bool>>? searchPredicate = null) where T : Entity
    {
        if (searchPredicate != null)
            source = source.ApplySearch(parameters, searchPredicate);

        return source;
    }
    public static async Task<PagedResult<T>> ApplyFiltersAndPageAsync<T>(
        this IQueryable<T> source,
        BaseQueryParameters parameters,
        Expression<Func<T, bool>>? searchPredicate = null,
        CancellationToken cancellationToken = default) where T : Entity
    {
        source = source.ApplyFilters(parameters, searchPredicate);
        return await source.ToPagedResultAsync(parameters, cancellationToken);
    }
    
    public static async Task<PagedResult<TResult>> ApplyFiltersAndPageAsync<T, TResult>(
        this IQueryable<T> source,
        BaseQueryParameters parameters,
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? searchPredicate = null,
        CancellationToken cancellationToken = default) where T : Entity
    {
        source = source.ApplyFilters(parameters, searchPredicate);
        return await source.ToPagedResultAsync(parameters, selector, cancellationToken);
    }
}