using System.Diagnostics.CodeAnalysis;

namespace DomainMediator.DataBase;

[ExcludeFromCodeCoverage]
public record PageQueryRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageLimit { get; set; } = 10;
}