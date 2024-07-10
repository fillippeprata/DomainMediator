#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.

using MediatR;

namespace DomainMediator.Queries;

public abstract class DomainQueryHandler<QueryT, ReturnT> : IRequestHandler<QueryT, ReturnT>
    where ReturnT : IQueryResponse where QueryT : IDomainQuery<ReturnT>
{
    public async Task<ReturnT?> Handle(QueryT request, CancellationToken cancellationToken)
    {
        return await Handle(request);
    }

    public abstract Task<ReturnT?> Handle(QueryT query);
}