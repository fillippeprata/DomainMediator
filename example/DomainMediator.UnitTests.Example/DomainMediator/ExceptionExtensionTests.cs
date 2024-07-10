using System.Diagnostics.CodeAnalysis;
using DomainMediator.Exceptions;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class ExceptionExtensionTests
{
    [Fact]
    public void RootExceptionText_WhenCalled_ReturnsRootExceptionText()
    {
        // Arrange
        var ex = new Exception("Root Exception");
        ex = new Exception("Inner Exception", ex);

        // Act
        var result = ex.RootExceptionText();

        // Assert
        Assert.Equal("Inner Exception -> Root Exception", result);
    }

    [Fact]
    public void RootExceptionText_WhenCalledWithNoInnerException_ReturnsMessage()
    {
        // Arrange
        var ex = new Exception("Root Exception");

        // Act
        var result = ex.RootExceptionText();

        // Assert
        Assert.Equal("Root Exception", result);
    }
}