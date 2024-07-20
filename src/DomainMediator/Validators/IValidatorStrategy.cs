namespace DomainMediator.Validators;

public interface IValidatorStrategy<in T>
{
    string Message { get; }
    bool Validate(T obj);
}
