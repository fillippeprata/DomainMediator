using DomainMediator.Commands;
using DomainMediator.Queries;

namespace DomainMediator.WebApi.WebRequests;

public abstract class DomainApi(Mediator _mediator)
{
    public Mediator Mediator => _mediator;

    public async Task<IResult> Exec(IDomainCommand command)
    {
        await _mediator.Exec(command);
        return Response();
    }

    public async Task<IResult> Exec<CommandResponseT>(IDomainCommand<CommandResponseT> command)
        where CommandResponseT : ICommandResponse
    {
        var result = await _mediator.Exec(command);
        return Response(result);
    }

    public async Task<IResult> Get<QueryResponseT>(IDomainQuery<QueryResponseT> query)
        where QueryResponseT : IQueryResponse
    {
        var result = await _mediator.Get(query);
        return Response(result);
    }

    public IResult Response()
    {
        return Results.Json(new ApiResponse(_mediator), statusCode: _mediator.GetHttpStatusCode());
    }

    private IResult Response<T>(T result)
    {
        var statusCode = _mediator.GetHttpStatusCode();

        return Results.Json(new ApiResponse<T?>(IsAValidCode() ? result : default, _mediator), statusCode: statusCode);

        bool IsAValidCode()
        {
            return statusCode is >= 200 and < 300;
        }
    }
}

internal class DomainApiImp(Mediator _mediator) : DomainApi(_mediator)
{
}
