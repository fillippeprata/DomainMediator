using FluentValidation;

namespace DomainMediator.Validators;

public static class FluentValidationExtensions
{
    public static void Validate<T, TProperty>(this IRuleBuilder<T, TProperty> builder,
        IValidatorStrategy<TProperty> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);
        builder.Must(validator.Validate).WithMessage(validator.Message);
    }

    public static void EmailMaxLength<T>(this IRuleBuilder<T, string?> builder)
    {
        builder.MaximumLength(254);
    }

    public static void ExactLength<T>(this IRuleBuilder<T, string?> builder, int length)
    {
        builder.MinimumLength(length);
        builder.MaximumLength(length);
    }
}
