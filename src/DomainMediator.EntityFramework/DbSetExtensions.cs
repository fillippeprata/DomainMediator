using System.Linq.Expressions;
using AutoMapper;
using DomainMediator.DataBase;
using DomainMediator.Queries;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework;

public static class DbSetExtensions
{
    public static PageResultResponse<TResponse> RunPagedQuery<T, TResponse>(this DbSet<T> dbSet, IMapper mapper, Expression<Func<T,bool>> predicate, PageQueryRequest queryRequest) where T: class
    {
        var result = dbSet.Where(predicate.Compile()).Skip(queryRequest.PageLimit * (queryRequest.PageNumber - 1))
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
