namespace DomainMediator.DataBase;

public record PageResultResponse<T>
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
