using System.Linq.Expressions;
using AutoMapper;
using DomainMediator.DataBase;

namespace DomainMediator.EntityFramework;

public static class DbSetExtensions
{
    public static PageResultResponse<TResponse> RunPagedQuery<TEntity, TResponse>(
        this IQueryable<TEntity> included,
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        PageQueryRequest queryRequest) where TEntity : class
    {
        var pageLimit = queryRequest.PageLimit ?? 10;
        var pageNumber = queryRequest.PageNumber ?? 1;
        var result = included.AsEnumerable().Where(predicate.Compile()).Skip(pageLimit * (pageNumber - 1))
            .Take(pageLimit);

        return new PageResultResponse<TResponse>
        {
            Data = mapper.Map<IEnumerable<TResponse>>(result),
            TotalItems = result.Count(predicate.Compile()),
            PageLimit = pageLimit,
            PageNumber = pageNumber
        };
    }
}
