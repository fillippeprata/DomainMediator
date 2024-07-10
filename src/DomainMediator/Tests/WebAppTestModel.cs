using System.Diagnostics.CodeAnalysis;

namespace DomainMediator.Tests;

[ExcludeFromCodeCoverage]
public record WebAppTestModel
{
    public TestHttpMethod HttpMethod { get; init; } = TestHttpMethod.Get;
    public bool EnsureSuccess { get; init; } = true;
    public object? Content { get; init; }
}