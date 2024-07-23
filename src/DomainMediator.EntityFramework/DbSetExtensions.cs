using System.Linq.Expressions;
using AutoMapper;
using DomainMediator.DataBase;
using DomainMediator.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DomainMediator.EntityFramework;

public static class DbSetExtensions
{
    public static PageResultResponse<TResponse> RunPagedQuery<TEntity, TResponse>(
        this IQueryable<TEntity> included,
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        PageQueryRequest queryRequest) where TEntity : class
    {
        var result = included.AsEnumerable().Where(predicate.Compile()).Skip(queryRequest.PageLimit * (queryRequest.PageNumber - 1))
            .Take(queryRequest.PageLimit);

        return new PageResultResponse<TResponse>
        {
            Data = mapper.Map<IEnumerable<TResponse>>(result),
            TotalItems = result.Count(predicate.Compile()),
            PageLimit = queryRequest.PageLimit,
            PageNumber = queryRequest.PageNumber
        };
    }
}
