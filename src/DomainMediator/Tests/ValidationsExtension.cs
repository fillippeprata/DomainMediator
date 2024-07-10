using FluentAssertions;
using FluentValidation.Results;

namespace DomainMediator.Tests;

public static class ValidationExtension
{
    public static void Assert(this ValidationResult result)
    {
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    public static void ValidateErrors(this ValidationResult result, params string[] errorMessage)
    {
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        foreach (var error in errorMessage)
            result.Errors.Select(x => x.ErrorMessage).Should().Contain(error);
    }
}