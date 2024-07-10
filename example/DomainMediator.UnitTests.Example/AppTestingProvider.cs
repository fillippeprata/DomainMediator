using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DomainMediator.UnitTests.Example;

[ExcludeFromCodeCoverage]
public class AppTestingProvider : WebApplicationFactory<WebApiExampleProgram> {}
