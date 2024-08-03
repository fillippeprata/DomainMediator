using System.Diagnostics.CodeAnalysis;

namespace DomainMediator.DataBase;

[ExcludeFromCodeCoverage]
public record PageQueryRequest
{
    public int? PageNumber { get; init; }
    public int? PageLimit { get; init; }
}
