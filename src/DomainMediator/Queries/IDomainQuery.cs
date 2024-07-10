using MediatR;

namespace DomainMediator.Queries;

public interface IDomainQuery<out ResponseT> : IRequest<ResponseT> where ResponseT : IQueryResponse
{
}