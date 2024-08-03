using System.Linq.Expressions;
using AutoMapper;
using DomainMediator.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework;

public static class DbSetExtensions
{
    public static PageResultResponse<TResponse> RunPagedQuery<TEntity, TResponse>(
        this IQueryable<TEntity> query,
        IMapper mapper,
        PageQueryRequest queryRequest,
        bool logSqlQuery = false) where TEntity : class
    {
        var pageLimit = queryRequest.PageLimit ?? 10;
        var pageNumber = queryRequest.PageNumber ?? 1;

        if (logSqlQuery)
        {
            var sqlQuery = query.ToQueryString();
            Console.WriteLine(sqlQuery);
        }

        var result = query.Skip(pageLimit * (pageNumber - 1))
            .Take(pageLimit);

        return new PageResultResponse<TResponse>
        {
            Data = mapper.Map<IEnumerable<TResponse>>(result),
            TotalItems = query.Count(),
            PageLimit = pageLimit,
            PageNumber = pageNumber
        };
    }
}
