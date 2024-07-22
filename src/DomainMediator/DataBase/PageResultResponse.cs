using DomainMediator.Queries;

namespace DomainMediator.DataBase;

public record PageResultResponse<T>: IQueryResponse
{
    public PageResultResponse(PageResultResponse<T> result)
    {
        PageNumber = result.PageNumber;
        PageLimit = result.PageLimit;
        TotalItems = result.TotalItems;
        Data = result.Data;
    }

    public int PageNumber { get; init; }
    public int PageLimit { get; init; }
    public int TotalItems { get; init; }
    public IEnumerable<T> Data { get; init; } = [];
}
